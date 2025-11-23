using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using Model.Data_Transfer_Objects;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using ViewModel;

namespace Server_Manager___API.Controllers
{
    // The route template is "api/SelectByIdx/[ActionName]"
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UpdateController : Controller
    {
        //--- ADMIN UPDATE ---
        [HttpPut]
        [ActionName("AdminUpdator")]
        public IActionResult UpdateAdmin([FromBody] AdminDTO admin)
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

                bool isModified = false;
                if (admin.Id != null)
                {
                    originalAdmin.Id = admin.Id;
                    isModified = true;
                }

                if (admin.Email != null)
                {
                    originalAdmin.Email = admin.Email;
                    isModified = true;
                }

                if (admin.Password != null)
                {
                    originalAdmin.Email = admin.Email;
                    isModified = true;
                }

                if (admin.Username != null)
                {
                    originalAdmin.Username = admin.Username;
                    isModified = true;
                }

                if (admin.StartDate != null)
                {
                    originalAdmin.StartDate = admin.StartDate;
                    isModified = true;
                }

                if (admin.Birthday != null)
                {
                    originalAdmin.Birthday = admin.Birthday;
                    isModified = true;
                }

                if (admin.Birthday != null)
                {
                    originalAdmin.Birthday = admin.Birthday;
                    isModified = true;
                }

                if (admin.IsLoggedIn != null && admin.IsLoggedIn.HasValue)
                {
                    originalAdmin.IsLoggedIn = admin.IsLoggedIn.Value;
                    isModified = true;
                }

                int changedRecords = 0;
                if (isModified)
                {
                    adminsDB.Update(originalAdmin);
                    changedRecords = adminsDB.SaveChanges();
                }

                if (changedRecords > 0)
                {
                    // Success with changes: 200 OK.
                    return StatusCode(200, $"OK: Record for Admin Idx=" +
                        $" {admin.Idx} successfully updated.\n" +
                        $" Records changed: {changedRecords}");
                }
                else
                {
                    // Success with no changes: 200 OK with a specific message.
                    return StatusCode(200, $"OK: Record for Admin Idx=" +
                        $"{admin.Idx} was not changed as the data was identical, " +
                        $"Records changed: {0}");
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
