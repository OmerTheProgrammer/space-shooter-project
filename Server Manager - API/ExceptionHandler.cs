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
                // - No SQL context provided = 404, resource not found or 422 for nested entity update
                if (string.IsNullOrEmpty(expandedEx.SqlErrorText))
                {

                    //if update a field (Not Idx) in a nested entity -> give 422
                    if (expandedEx.Message.Contains("Invalid Use of Update"))
                    {
                        statusCode = StatusCodes.Status422UnprocessableEntity;
                        responseMessage = "Client Bad Request About Nested Entity Error: " + expandedEx.Message;
                    }
                    else
                    {
                        // Regular 404 Not Found case
                        statusCode = StatusCodes.Status404NotFound;
                        responseMessage = expandedEx.Message; // Use the specific "Not Found" message

                        // Log at Warning level since this is an expected business logic failure
                        _logger.LogWarning("404 - Resource Not Found: {Message}", responseMessage);
                    }
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
                //context.Response.ContentType = "text/plain";
                context.Response.StatusCode = statusCode;

                var result = JsonConvert.SerializeObject(new { StatusCode = statusCode, Message = responseMessage });
                return context.Response.WriteAsync(result);
                //return context.Response.WriteAsync(responseMessage);
            }

            // 2. DATABASE ERROR ANALYSIS (Only runs for non-404-Errors)
            //turns the actual db error message to string for analysis
            string ErrorMessage = errorToAnalyze.InnerException?.Message ?? errorToAnalyze.Message;

            // 2a. UNIQUE KEY VIOLATION (409 Conflict)
            if (ErrorMessage.Contains("duplicate key"))
            {
                statusCode = StatusCodes.Status409Conflict;
                string pattern = @"Unique_(\w+)_(\w+)";
                Match match = Regex.Match(ErrorMessage, pattern);

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
            else if (ErrorMessage.Contains("FOREIGN KEY constraint") ||
                     ErrorMessage.Contains("REFERENCE constraint"))
            {
                bool IsForeignKeyViolation =
                    ErrorMessage.Contains("FOREIGN KEY constraint");
                //if ForeignKeyViolation -> 400, else (refreance) 409
                statusCode = IsForeignKeyViolation ?
                    StatusCodes.Status400BadRequest :
                    StatusCodes.Status409Conflict;

                Match constraintMatch = Regex.Match(ErrorMessage,
                    @"constraint\s+\""(\w+)\""");

                Match tableColumnMatch = Regex.Match(ErrorMessage,
                    @"table\s+\""dbo\.(\w+)\""\,\s+column\s+'(\w+)'");

                // Extract details or use generic terms
                string constraintName = constraintMatch.Success ?
                    constraintMatch.Groups[1].Value :
                    "a foreign key constraint";

                string tableName = tableColumnMatch.Success ?
                    tableColumnMatch.Groups[1].Value :
                    "another table";
                string columnName = tableColumnMatch.Success ?
                    tableColumnMatch.Groups[2].Value :
                    "a required column";

                if (IsForeignKeyViolation)
                {
                    responseMessage = $"Bad Request: " +
                        $"Cannot process operation due to missing " +
                        $"referenced data in {columnName} of table {tableName}" +
                        $"Foreign Key violation: {constraintName}).";
                }
                else
                {
                    responseMessage = $"Conflict: record is still referenced " +
                        $"by the '{columnName}' " +
                        $"column in the '{tableName}' table " +
                        $"(Constraint: {constraintName}).";
                }
            }

            // 2c. NOT NULL VIOLATION (400 Bad Request)
            else if (ErrorMessage.Contains("NULL into column"))
            {
                statusCode = StatusCodes.Status400BadRequest;
                string nullColumnPattern = @"column\s+\""(\w+)\""";
                Match nullMatch = Regex.Match(ErrorMessage, nullColumnPattern);
                string columnName = nullMatch.Success ? nullMatch.Groups[1].Value : "a mandatory field";
                responseMessage = $"Bad Request: The mandatory field '{columnName}' was not provided (NOT NULL violation).";
            }

            // Fallback for general errors remains 500 (set at the beginning)

            // 3. Send the final response

            context.Response.ContentType = "application/json";
            //context.Response.ContentType = "text/plain";
            context.Response.StatusCode = statusCode;

            var finalResult = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                Message = responseMessage
            });
            return context.Response.WriteAsync(finalResult);
            //return context.Response.WriteAsync(responseMessage);
        }
    }
}
