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
                if (ChangedRecords == 0)//mean's "not found" and didn't delete
                {
                    return NotFound($"Didn't find Admin: idx = {idx}, so didn't delete!");
                }
                return Ok(ChangedRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }


    }
}