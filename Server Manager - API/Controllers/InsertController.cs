using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using System.Text.RegularExpressions;
using ViewModel.DBs;

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
                AdminsDB adminsDB = new AdminsDB();
                adminsDB.Insert(admin);
                int ChangedRecords = adminsDB.SaveChanges();
                // 200 - OK
                return StatusCode(200, ChangedRecords);
        }

        [HttpPost]
        [ActionName("UsersInsertor")]
        public IActionResult InsertUser([FromBody] User user)
        {
            UsersDB usersDB = new UsersDB();
            usersDB.Insert(user);
            int changedRecords = usersDB.SaveChanges();
            // 200 - OK: 
            return StatusCode(200, changedRecords);
        }
    }
}