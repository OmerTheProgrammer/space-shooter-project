using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using System;
using System.Text.RegularExpressions;
using ViewModel;

namespace Server_Manager___API.Controllers
{
    // The route template is "api/SelectByIdx/[ActionName]"
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UpdateController : Controller
    {
        // Define the specific default/sentinel values used by all entities
        private static readonly DateTime[] DEFAULT_DATEs = DateTime.Now, new DateTime(1753, 1, 1, 12, 0, 0), ;
        private const string[] DEFAULT_STRINGs = new string[]{"","string" };
        private const bool DEFAULT_BOOL = false;
        // --- ADMIN UPDATE ---
        [HttpPut]
        [ActionName("AdminUpdator")]
        public IActionResult UpdateAdmin([FromBody] Admin admin)
        {
            try
            {
                //run only thruogh Client side becouse replaces every field!
                AdminsDB adminsDB = new AdminsDB();
                //get current db values
                Admin originalAdmin = AdminsDB.SelectByIdx(admin.Idx);

                if (originalAdmin == null)
                {
                    return StatusCode(404, $"Admin with Idx={admin.Idx} not found.");
                }

                //easy primitve fields
                if (DEFAULT_STRINGs.Contains(admin.Id))
                {
                    originalAdmin.Id = admin.Id;
                }
                if (admin.Password != DEFAULT_STRING)
                {
                    originalAdmin.Password = admin.Password;
                }
                if (admin.Username != DEFAULT_STRING)
                {
                    originalAdmin.Username = admin.Username;
                }
                if (admin.Email != DEFAULT_STRING)
                {
                    originalAdmin.Email = admin.Email;
                }

                // --- DateTime? Fields (Default: 1753-01-01 12:00:00) ---
                // You must check both: HasValue AND if the value is not the sentinel date.
                if (admin.Birthday.HasValue && admin.Birthday.Value != DEFAULT_DATE)
                {
                    originalAdmin.Birthday = admin.Birthday;
                }
                if (admin.StartDate.HasValue && admin.StartDate.Value != DEFAULT_DATE)
                {
                    originalAdmin.StartDate = admin.StartDate;
                }

                // If 'false' is a valid, intentional update, you need to use a different DTO.
                if (admin.IsLoggedIn != DEFAULT_BOOL)
                {
                    originalAdmin.IsLoggedIn = admin.IsLoggedIn;
                }

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
                    return StatusCode(200, $"OK: Record for Admin Idx=" +
                        $"{admin.Idx} didn't need to change, " +
                        $"no returned content.\n" +
                        $" Records changed: {0}");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException?.Message ?? ex.Message;
                // Check for the specific "not found" message from the DB layer.
                if (errorMessage.Contains("not found"))
                {
                    // Use 404 Not Found for missing resources, with the concise error message.
                    return StatusCode(404, ex.Message);
                }

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
