using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using Model.Data_Transfer_Objects;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using ViewModel.DBs;

namespace Server_Manager___API.Controllers
{

    // The route template is "api/Update/[ActionName]"
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UpdateController : Controller
    {
        public static bool TryUpdateProperty<T>(T? source, Action<T> setter) 
            where T : class //like string
        {
            // This handles strings and other nullable reference types
            if (source != null)
            {
                setter(source);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Overload for Nullable Value Types (DateTime?, bool?).
        /// </summary>
        public static bool TryUpdateProperty<T>(T? source, Action<T> setter)
            where T : struct //like int, DateTime, bool and rest
        {
            // This handles DateTime?, bool?, int?, etc.
            if (source.HasValue)
            {
                setter(source.Value);
                return true;
            }
            return false;
        }

        //--- ADMIN UPDATE ---
        [HttpPut]
        [ActionName("AdminUpdator")]
        public IActionResult UpdateAdmin([FromBody] AdminDTO admin)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            AdminsDB adminsDB = new AdminsDB();

            // 1. Fetch current DB values. If not found, AdminsDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            Admin originalAdmin = AdminsDB.SelectByIdx(admin.Idx);

            bool isModified = false;

            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            isModified |= TryUpdateProperty(admin.Id, val => originalAdmin.Id = val);
            isModified |= TryUpdateProperty(admin.Email, val => originalAdmin.Email = val);
            isModified |= TryUpdateProperty(admin.Password, val => originalAdmin.Password = val);
            isModified |= TryUpdateProperty(admin.Username, val => originalAdmin.Username = val);

            // 2. Nullable Value Types (DateTime?, bool?)
            isModified |= TryUpdateProperty(admin.StartDate, val => originalAdmin.StartDate = val);
            isModified |= TryUpdateProperty(admin.Birthday, val => originalAdmin.Birthday = val);
            isModified |= TryUpdateProperty(admin.IsLoggedIn, val => originalAdmin.IsLoggedIn = val);

            int changedRecords = 0;
            if (isModified)
            {
                adminsDB.Update(originalAdmin);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = adminsDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for Admin Idx=" +
                    $" {admin.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for Admin Idx=" +
                    $"{admin.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }


    }
}
