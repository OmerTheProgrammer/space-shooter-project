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
                return Ok(ChangedRecords);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (errorMessage.Contains("Violation of UNIQUE KEY constraint") && errorMessage.Contains("Cannot insert duplicate key"))
                {

                    // Define the regular expression pattern to find the constraint name
                    // Pattern: UQ_ (literal), then capture group 1 (Table Name), then _ (literal), then capture group 2 (Field Name)
                    // The pattern looks for a constraint name starting with 'UQ_'
                    string pattern = @"Unique_(\w+)_(\w+)";

                    Match match = Regex.Match(errorMessage, pattern);

                    if (match.Success)
                    {
                        // Extract the captured groups
                        string ConstarinOriginalTable = match.Groups[1].Value; // Group 1: e.g., "Users"
                        string duplicateField = match.Groups[2].Value; // Group 2: e.g., "ID", "Username", "Email"

                        // Return 409 Conflict with the specific table and field information
                        return StatusCode(409, $"The insertion failed: The **{duplicateField}**" +
                            $" you provided already exists in the **{ConstarinOriginalTable}**" +
                            $" table. Please enter a unique value.");
                    }
                }
                // Use 500 Internal Server Error for all other unexpected issues.
                return StatusCode(500, $"An unexpected server error occurred: {errorMessage}");
            }
        }


    }
}