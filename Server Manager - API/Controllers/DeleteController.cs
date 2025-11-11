using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using ViewModel;

namespace Server_Manager___API.Controllers
{
    // The route template is "api/SelectByIdx/[ActionName]"
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeleteController : Controller
    {
        // --- ADMINS ---
        [HttpDelete]
        [ActionName("AdminDeletor")]
        // NOTE: Changed parameter name from InsertAdmin to AdminDeletor for clarity.
        public IActionResult AdminDeletor([FromBody] int idx)
        {
            try
            {
                AdminsDB adminsDB = new AdminsDB();
                adminsDB.Delete(new Admin { Idx = idx });
                int changedRecords = adminsDB.SaveChanges();

                if (changedRecords == 0) // Resource not found
                {
                    // 404 Not Found
                    return StatusCode(404, $"Not Found: Admin with idx = {idx} was not found.");
                }

                return StatusCode(200, $"OK: Record for Admin Idx=" +
                        $"{idx} was removed.\n" +
                        $" Records changed: {changedRecords}");
            }
            catch (Exception ex)
            {
                //tries to get the innermost exception message,
                //becouse errors in SQL are often wrapped in c# errors.
                string errorMessage = ex.InnerException?.Message ?? ex.Message;

                // 1. FOREIGN KEY VIOLATION (409 Conflict or 400 Bad Request)
                // This occurs when a child record (e.g., an 'Order' created by this Admin) exists.
                if (errorMessage.Contains("FOREIGN KEY constraint") ||
                    errorMessage.Contains("violates foreign key constraint"))
                {
                    // 409 Conflict is often preferred for state-based conflicts.
                    return StatusCode(409,
                        $"Conflict: Cannot delete Admin with idx = {idx} because it is referenced by other records (Foreign Key violation).");
                }

                // 2. GENERAL SERVER ERROR (500 Internal Server Error)
                return StatusCode(500, $"Internal Server Error: {errorMessage}");
            }
        }
    }
}