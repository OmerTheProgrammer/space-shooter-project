using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using ViewModel;
using ViewModel.DBs;

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
        public IActionResult AdminDeletor([FromBody] int idx)
        {
            AdminsDB adminsDB = new AdminsDB();
            adminsDB.Delete(new Admin { Idx = idx });
            int changedRecords = adminsDB.SaveChanges();

            if (changedRecords == 0) // Resource not found or already deleted
            {
                // Throw ExpandedException without SQL context. The middleware will catch this
                // and return a 404 Not Found (based on Case A logic in the handler).
                throw new ExpandedException($"Not Found: Admin with idx = {idx} was not found or has already been deleted.");
            }

            return StatusCode(200, $"OK: Record for Admin Idx=" +
                $"{idx} was removed.\n" +
                $" Records changed: {changedRecords}");
        }

        // --- USERS ---
        [HttpDelete]
        [ActionName("UserDeletor")]
        public IActionResult UserDeletor([FromBody] int idx)
        {
            UsersDB usersDB = new UsersDB();
            usersDB.Delete(new User { Idx = idx });
            int changedRecords = usersDB.SaveChanges();

            if (changedRecords == 0) // Resource not found or already deleted
            {
                // Throw ExpandedException without SQL context. The middleware will catch this
                // and return a 404 Not Found (based on Case A logic in the handler).
                throw new ExpandedException($"Not Found: User with idx = {idx} was not found or has already been deleted.");
            }

            return StatusCode(200, $"OK: Record for User Idx=" +
                $"{idx} was removed.\n" +
                $" Records changed: {changedRecords}");
        }


    }
}