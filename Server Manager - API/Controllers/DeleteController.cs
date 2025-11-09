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
        public IActionResult InsertAdmin([FromBody] int idx)
        {
            try
            {
                AdminsDB adminsDB = new AdminsDB();
                adminsDB.Delete(new Admin { Idx = idx});
                int ChangedRecords = adminsDB.SaveChanges();
                return Ok(ChangedRecords);
            }
            catch (Exception ex)
            {
                // Check for the specific "not found" message from the DB layer.
                if (ex.Message.Contains("not found"))
                {
                    // Use 404 Not Found for missing resources, with the concise error message.
                    return NotFound(ex.Message);
                }
                // Use 500 Internal Server Error for all other unexpected issues.
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }


    }
}