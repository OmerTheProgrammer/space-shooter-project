using Newtonsoft.Json;
using System.Text.RegularExpressions;
using ViewModel;

namespace Server_Manager___API
{
    //class to run error handling as middleware
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        //Constructor to initialize the middleware with the next delegate and logger
        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        // The main method that gets called for each HTTP request as middleware
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //runs the request
                await _next(context);
            }
            catch (Exception ex)
            {
                // runs the exception handler if the request throws an exception
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Default to a general server error response
            int statusCode = StatusCodes.Status500InternalServerError;
            string responseMessage = "An unexpected error occurred.";
            Exception errorToAnalyze = exception;

            // 1. CHECK FOR EXPANDED EXCEPTION (Custom Logic) - errors from VM
            //means or 404 errors or db errors or just general logging
            if (exception is ExpandedException expandedEx)
            {
                // Case A: Entity.SelectByIdx error = (404 Not Found)
                // - No SQL context provided from VM
                if (string.IsNullOrEmpty(expandedEx.SqlErrorText))
                {
                    statusCode = StatusCodes.Status404NotFound;
                    responseMessage = expandedEx.Message; // Use the specific "Not Found" message

                    // Log at Warning level since this is an expected business logic failure
                    _logger.LogWarning("404 - Resource Not Found: {Message}", responseMessage);
                }
                // Case B: BaseDB error: (409/400/500) - SQL context is present
                else
                {
                    // Log the custom context (SQL statement)
                    string customLogMessage = $"Error: {expandedEx.Message}. SQL TEXT: {expandedEx.SqlErrorText}";
                    _logger.LogError(expandedEx, customLogMessage);

                    // Set the error analyzed later to the inner exception for DB analysis
                    //if no inner exception, use the expanded exception itself
                    errorToAnalyze = expandedEx.InnerException ?? expandedEx;
                }
            }
            else
            {
                // Case C: General Exception (500 Internal Server Error)
                // If it's a completely unhandled application error, log it as critical.
                _logger.LogError(exception, "Uncaught application error occurred during request pipeline.");
                // Use the general exception message for the client response
                responseMessage = $"Internal Server Error: {exception.Message}";
            }

            // --- If a 404 was determined, we skip DB analysis and return immediately ---
            if (statusCode == StatusCodes.Status404NotFound)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;
                var result = JsonConvert.SerializeObject(new { StatusCode = statusCode, Message = responseMessage });
                return context.Response.WriteAsync(result);
            }

            // 2. DATABASE ERROR ANALYSIS (Only runs for non-404-Errors)
            //turns the actual db error message to string for analysis
            string dbErrorMessage = errorToAnalyze.InnerException?.Message ?? errorToAnalyze.Message;

            // 2a. UNIQUE KEY VIOLATION (409 Conflict)
            if (dbErrorMessage.Contains("duplicate key"))
            {
                statusCode = StatusCodes.Status409Conflict;
                string pattern = @"Unique_(\w+)_(\w+)";
                Match match = Regex.Match(dbErrorMessage, pattern);

                if (match.Success)
                {
                    string table = match.Groups[1].Value;
                    string field = match.Groups[2].Value;
                    responseMessage = $"Conflict: The {field} already exists in the {table}.";
                }
                else
                {
                    responseMessage = "Conflict: A unique constraint was violated.";
                }
            }

            // 2b. FOREIGN KEY VIOLATION (400 Bad Request)
            else if (dbErrorMessage.Contains("FOREIGN KEY constraint"))
            {
                statusCode = StatusCodes.Status400BadRequest;
                string fkConstraintPattern = @"constraint\s+\""(\w+)\""";
                Match fkMatch = Regex.Match(dbErrorMessage, fkConstraintPattern);
                string constraintName = fkMatch.Success ? fkMatch.Groups[1].Value : "a foreign key constraint";
                responseMessage = $"Bad Request: Cannot process operation due to missing referenced data (Foreign Key violation: {constraintName}).";
            }

            // 2c. NOT NULL VIOLATION (400 Bad Request)
            else if (dbErrorMessage.Contains("NULL into column"))
            {
                statusCode = StatusCodes.Status400BadRequest;
                string nullColumnPattern = @"column\s+\""(\w+)\""";
                Match nullMatch = Regex.Match(dbErrorMessage, nullColumnPattern);
                string columnName = nullMatch.Success ? nullMatch.Groups[1].Value : "a mandatory field";
                responseMessage = $"Bad Request: The mandatory field '{columnName}' was not provided (NOT NULL violation).";
            }
            // Fallback for general errors remains 500 (set at the beginning)

            // 3. Send the final response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var finalResult = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                Message = responseMessage
            });

            return context.Response.WriteAsync(finalResult);
        }
    }
}
