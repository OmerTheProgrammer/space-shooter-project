using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using System.Text.RegularExpressions;
using ViewModel;

namespace Server_Manager___API.Controllers
{
    // The route template is "api/SelectByIdx/[ActionName]"
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UpdateController : Controller
    {
        // --- ADMIN UPDATE ---
        [HttpPut]
        [ActionName("AdminUpdator")]
        public IActionResult UpdateAdmin([FromBody] Admin admin)
        {
            try
            {
                AdminsDB adminsDB = new AdminsDB();
                adminsDB.Update(admin);
                int changedRecords = adminsDB.SaveChanges();

                if (changedRecords > 0)
                {
                    // Success with changes: 200 OK.
                    return StatusCode(200, $"OK: Record for Admin Idx=" +
                        $" {admin.Idx} successfully updated.\n" +
                        $" Records changed: {changedRecords}");
                }
                else
                {
                    // Success with no changes: 204 No Content.
                    return StatusCode(204, $"OK: Record for Admin Idx=" +
                        $"{admin.Idx} didn't need to change, " +
                        $"no returned content.\n" +
                        $" Records changed: {changedRecords}");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException?.Message ?? ex.Message;

                // 1. UNIQUE KEY VIOLATION (409 Conflict)
                if (errorMessage.Contains("duplicate key") ||
                    errorMessage.Contains("Duplicate entry") ||
                    errorMessage.Contains("unique constraint failed") ||
                    errorMessage.Contains("violates unique constraint")) // Expanded terms
                {
                    string pattern = @"Unique_(\w+)_(\w+)";
                    Match match = Regex.Match(errorMessage, pattern);

                    if (match.Success)
                    {
                        string field = match.Groups[2].Value;
                        string table = match.Groups[1].Value;
                        return StatusCode(409, $"Conflict: The {field} already exists in the {table}.");
                    }
                    return StatusCode(409, "Conflict: A unique constraint was violated during update.");
                }

                // 2. NOT NULL VIOLATION (400 Bad Request)
                if (errorMessage.Contains("NULL into column") ||
                    errorMessage.Contains("may not be NULL") ||
                    errorMessage.Contains("violates not-null constraint") ||
                    errorMessage.Contains("column does not allow nulls")) // Expanded terms
                {
                    return StatusCode(400, "Bad Request: A mandatory field was not provided (NOT NULL violation).");
                }

                // 3. CHECK CONSTRAINT VIOLATION (400 Bad Request)
                if (errorMessage.Contains("CHECK constraint") ||
                    errorMessage.Contains("check constraint failed") ||
                    errorMessage.Contains("violates check constraint")) // Expanded terms
                {
                    return StatusCode(400, "Bad Request: The data violates a defined business rule (Check constraint).");
                }

                // 4. FOREIGN KEY VIOLATION (400 Bad Request)
                if (errorMessage.Contains("FOREIGN KEY constraint") ||
                    errorMessage.Contains("violates foreign key constraint") ||
                    errorMessage.Contains("reference constraint failed")) // Expanded terms
                {
                    return StatusCode(400, "Bad Request: Referenced entity does not exist (Foreign Key violation).");
                }

                // 5. GENERAL SERVER ERROR (500)
                return StatusCode(500, $"Internal Server Error: {errorMessage}");
            }
        }
    }
}
