using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
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
                // Use 500 Internal Server Error for all other unexpected issues.
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }


    }
}