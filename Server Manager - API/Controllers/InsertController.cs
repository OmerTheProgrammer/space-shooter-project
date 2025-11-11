using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using System.Text.RegularExpressions;
using ViewModel;

namespace Server_Manager___API.Controllers
{
    // The route template is "api/SelectByIdx/[ActionName]"
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InsertController : Controller
    {
        // --- ADMINS ---
        [HttpPost]
        [ActionName("AdminsInsertor")]
        public IActionResult InsertAdmin([FromBody] Admin admin)
        {
            try
            {
                AdminsDB adminsDB = new AdminsDB();
                adminsDB.Insert(admin);
                int ChangedRecords = adminsDB.SaveChanges();
                // 200 - OK
                return StatusCode(200, ChangedRecords);
            }
            catch (Exception ex)
            {
                //tries to get the innermost exception message,
                //becouse errors in SQL are often wrapped in c# errors.
                string errorMessage = ex.InnerException?.Message ?? ex.Message;

                // 1. UNIQUE KEY VIOLATION (409 Conflict)
                if (errorMessage.Contains("duplicate key"))
                {
                    // Extract field and table information using regex
                    // Example pattern: "Unique_TableName_FieldName"
                    string pattern = @"Unique_(\w+)_(\w+)";
                    Match match = Regex.Match(errorMessage, pattern);

                    if (match.Success)
                    {
                        string field = match.Groups[2].Value;
                        string table = match.Groups[1].Value;
                        return StatusCode(409, $"Conflict: The {field} already exists in the {table}.");
                    }
                    return StatusCode(409, "Conflict: A unique constraint was violated.");
                }

                // 2. FOREIGN KEY VIOLATION (400 Bad Request)
                if (errorMessage.Contains("FOREIGN KEY constraint"))
                {
                    return StatusCode(400, "Bad Request: Referenced record does not exist (Foreign Key violation).");
                }

                // 3. NOT NULL VIOLATION (400 Bad Request)
                if (errorMessage.Contains("NULL into column"))
                {
                    return StatusCode(400, "Bad Request: A mandatory field was not provided (NOT NULL violation).");
                }

                // 4. GENERAL SERVER ERROR (500 Internal Server Error)
                return StatusCode(500, $"Internal Server Error: {errorMessage}");
            }
        }


    }
}