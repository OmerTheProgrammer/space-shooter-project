using Newtonsoft.Json;
using System.Text.RegularExpressions;

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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string errorMessage = exception.InnerException?.Message ?? exception.Message;
            int statusCode = StatusCodes.Status500InternalServerError;
            string responseMessage = $"Internal Server Error: {errorMessage}";

            // 1. UNIQUE KEY VIOLATION (409 Conflict)
            if (errorMessage.Contains("duplicate key"))
            {
                statusCode = StatusCodes.Status409Conflict;
                string pattern = @"Unique_(\w+)_(\w+)";
                Match match = Regex.Match(errorMessage, pattern);

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

            // 2. FOREIGN KEY VIOLATION (400 Bad Request)
            else if (errorMessage.Contains("FOREIGN KEY constraint"))
            {
                statusCode = StatusCodes.Status400BadRequest;

                // Extract the constraint name (e.g., FK_RunInfo_Player) for more detail
                string fkConstraintPattern = @"constraint\s+\""(\w+)\""";
                Match fkMatch = Regex.Match(errorMessage, fkConstraintPattern);

                string constraintName = fkMatch.Success ? fkMatch.Groups[1].Value : "a foreign key constraint";

                responseMessage = $"Bad Request: Cannot process operation due to missing referenced data (Foreign Key violation: {constraintName}).";
            }

            // 3. NOT NULL VIOLATION (400 Bad Request) - Common SQL Server message for a column expecting a value
            else if (errorMessage.Contains("NULL into column"))
            {
                statusCode = StatusCodes.Status400BadRequest;

                // Attempt to extract the column name for a more useful error message
                string nullColumnPattern = @"column\s+\""(\w+)\""";
                Match nullMatch = Regex.Match(errorMessage, nullColumnPattern);

                string columnName = nullMatch.Success ? nullMatch.Groups[1].Value : "a mandatory field";

                responseMessage = $"Bad Request: The mandatory field '{columnName}' was not provided (NOT NULL violation).";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Serialize the structured error response
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                Message = responseMessage
            });

            return context.Response.WriteAsync(result);
        }
    }
}
