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
        // Define the specific default/Basic_CS values used by all entities
        private static readonly DateTime DEFAULT_CS_DATE = new DateTime(1753, 1, 1, 12, 0, 0);
        private readonly string[] DEFAULT_STRINGs = new string[] { "", "string" };

        /// <summary>
        /// Determines if the incoming string value is meaningful for an update 
        /// (i.e., not null and not a placeholder default value).
        /// </summary>
        private bool IsMeaningfulStringUpdate(string dtoValue)
        {
            // Must be provided (not null) and must not be a placeholder default string
            return dtoValue != null && !DEFAULT_STRINGs.Contains(dtoValue);
        }

        /// <summary>
        /// Updates a string field using a setter delegate.
        /// </summary>
        private void UpdateStringField(string originalValue, string dtoValue,
            Action<string> setter, ref bool isModified)
        {
            if (IsMeaningfulStringUpdate(dtoValue))
            {
                if (originalValue != dtoValue)
                {
                    setter(dtoValue);
                    isModified = true;
                }
            }
        }

        /// <summary>
        /// Updates a boolean field using a setter delegate.
        /// </summary>
        private void UpdateBooleanField(bool originalValue, bool? dtoValue,
            Action<bool> setter, ref bool isModified)
        {
            // The check for HasValue is the primary validation here, as `false` is a valid update value.
            if (dtoValue.HasValue)
            {
                if (originalValue != dtoValue.Value)
                {
                    setter(dtoValue.Value);
                    isModified = true;
                }
            }
        }

        /// <summary>
        /// Determines if the incoming DateTime value is meaningful for an update.
        /// (i.e., HasValue, not the Basic_CS default, and not a serializer default near DateTime.Now).
        /// </summary>
        private bool IsMeaningfulDateTimeUpdate(DateTime? dtoValue)
        {
            if (!dtoValue.HasValue)
            {
                return false;
            }

            // 1. Check for the hardcoded Basic_CS date default
            bool isBasic_CS_Date = (dtoValue.Value == DEFAULT_CS_DATE);
            if (isBasic_CS_Date)
            {
                return false;
            }

            // 2. Check if the date is very close to now (serializer default)
            // Use 5 seconds as the margin for serializer delay.
            bool isCloseToNow = (Math.Abs(
                    (dtoValue.Value - DateTime.Now).TotalSeconds
                ) < 5);

            if (isCloseToNow)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates a nullable DateTime field using a setter delegate.
        /// </summary>
        private void UpdateDateTimeField(DateTime? originalValue,
            DateTime? dtoValue, Action<DateTime?> setter, ref bool isModified)
        {
            if (IsMeaningfulDateTimeUpdate(dtoValue))
            {
                if (originalValue != dtoValue.Value)
                {
                    setter(dtoValue.Value);
                    isModified = true;
                }
            }
        }

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
                // --- 1. String Fields ---
                UpdateStringField(
                     originalAdmin.Id,
                     admin.Id,
                     val => originalAdmin.Id = val,
                     ref isModified
                 );
                UpdateStringField(
                    originalAdmin.Password,
                    admin.Password,
                    val => originalAdmin.Password = val,
                     ref isModified
                );
                UpdateStringField(
                    originalAdmin.Username,
                    admin.Username,
                    val => originalAdmin.Username = val,
                     ref isModified
                );
                UpdateStringField(
                    originalAdmin.Email,
                    admin.Email,
                    val => originalAdmin.Email = val,
                    ref isModified
                );

                // --- 2. DateTime Fields ---
                // updates DateTime fields that aren't null
                UpdateDateTimeField(
                    originalAdmin.Birthday,
                    admin.Birthday,
                    val => originalAdmin.Birthday = val,
                    ref isModified
                );
                UpdateDateTimeField(
                    originalAdmin.StartDate,
                    admin.StartDate,
                    val => originalAdmin.StartDate = val,
                    ref isModified
                );

                // updates boolean fields that aren't null
                UpdateBooleanField(
                    originalAdmin.IsLoggedIn,
                    admin.IsLoggedIn,
                    val => originalAdmin.IsLoggedIn = val,
                    ref isModified
                );

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
