using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using System.Text.RegularExpressions;
using ViewModel.DBs;

namespace Server_Manager___API.Controllers
{
    // The route template is "api/Insert/[ActionName]"
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
        [ActionName("GroupsInsertor")]
        public IActionResult InsertUser([FromBody] Model.Entitys.Group group)
        {
            GroupsDB groupsDB = new GroupsDB();
            groupsDB.Insert(group);
            int changedRecords = groupsDB.SaveChanges();
            // 200 - OK: 
            return StatusCode(200, changedRecords);
        }

        [HttpPost]
        [ActionName("PlayersAndGroupsInsertor")]
        public IActionResult InsertPlayerAndGroup([FromBody]
            PlayerAndGroup playerAndGroup)
        {
            PlayersAndGroupsDB playerAndGroupsDB =
                new PlayersAndGroupsDB();
            playerAndGroupsDB.Insert(playerAndGroup);
            int changedRecords = playerAndGroupsDB.SaveChanges();
            // 200 - OK: 
            return StatusCode(200, changedRecords);
        }

        [HttpPost]
        [ActionName("PlayersInsertor")]
        public IActionResult InsertPlayer([FromBody] Player player)
        {
            PlayersDB playersDB = new PlayersDB();
            playersDB.Insert(player);
            int changedRecords = playersDB.SaveChanges();
            // 200 - OK: 
            return StatusCode(200, changedRecords);
        }

        [HttpPost]
        [ActionName("ProfileEditRequestsInsertor")]
        public IActionResult InsertProfileEditRequest(
            [FromBody] ProfileEditRequest profileEditRequest)
        {
            ProfileEditRequestsDB profileEditRequestsDB = new ProfileEditRequestsDB();
            profileEditRequestsDB.Insert(profileEditRequest);
            int changedRecords = profileEditRequestsDB.SaveChanges();
            // 200 - OK: 
            return StatusCode(200, changedRecords);
        }

        [HttpPost]
        [ActionName("RequestsDataInsertor")]
        public IActionResult InsertRequestData([FromBody] RequestData requestData)
        {
            RequestsDataDB requestsDataDB = new RequestsDataDB();
            requestsDataDB.Insert(requestData);
            int changedRecords = requestsDataDB.SaveChanges();
            // 200 - OK: 
            return StatusCode(200, changedRecords);
        }

        [HttpPost]
        [ActionName("RunsInfoInsertor")]
        public IActionResult InsertRunInfo([FromBody] RunInfo runInfo)
        {
            RunsInfoDB runInfosDB = new RunsInfoDB();
            runInfosDB.Insert(runInfo);
            int changedRecords = runInfosDB.SaveChanges();
            // 200 - OK: 
            return StatusCode(200, changedRecords);
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