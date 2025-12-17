using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using ViewModel.DBs;

namespace Server_Manager___API.Controllers
{
    // The route template is "api/SelectByIdx/[ActionName]"
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SelectByIdxController : Controller
    {
        // --- ADMINS ---
        [HttpPost]
        [ActionName("AdminsSelectorByIdx")]
        public IActionResult SelectAdminByIdx([FromBody] int Idx)
        {
            // 1. Remove try/catch entirely.
            // 2. The ExpandedException thrown by AdminsDB.SelectByIdx(Idx)
            //    will automatically flow up to the ExceptionHandler middleware.

            Admin result = AdminsDB.SelectByIdx(Idx);

            // If SelectByIdx returns the entity (no exception thrown), return HTTP 200 OK.
            return StatusCode(200, result);
        }

        // --- ENEMIES IN LAST LEVEL ---
        [HttpPost]
        [ActionName("EnemiesInLastLevelSelectorByIdx")]
        public IActionResult SelectEnemyInLastLevelByIdx([FromBody] int Idx)
        {
            EnemyInLastLevel result = EnemiesInLastLevelDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- GROUPS ---
        [HttpPost]
        [ActionName("GroupsSelectorByIdx")]
        public IActionResult SelectGroupByIdx([FromBody] int Idx)
        {
            Group result = GroupsDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- PLAYERS AND GROUPS ---
        [HttpPost]
        [ActionName("PlayersAndGroupsSelectorByIdx")]
        public IActionResult SelectPlayerAndGroupByIdx(
            [FromBody] int Idx) {
            PlayerAndGroup result = PlayersAndGroupsDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- PLAYERS ---
        [HttpPost]
        [ActionName("PlayersSelectorByIdx")]
        public IActionResult SelectPlayerByIdx([FromBody] int Idx)
        {
            Player result = PlayersDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- PROFILE EDIT REQUESTS ---
        [HttpPost]
        [ActionName("ProfileEditRequestsSelectorByIdx")]
        public IActionResult SelectProfileEditRequestByIdx([FromBody] int Idx)
        {
            ProfileEditRequest result = ProfileEditRequestsDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- REQUESTS DATA ---
        [HttpPost]
        [ActionName("RequestsDataSelectorByIdx")]
        public IActionResult SelectRequestDataByIdx([FromBody] int Idx)
        {
            RequestData result = RequestsDataDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- RUN INFO ---
        [HttpPost]
        [ActionName("RunsInfoSelectorByIdx")]
        public IActionResult SelectRunInfoByIdx([FromBody] int Idx)
        {
            RunInfo result = RunsInfoDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- USERS ---
        [HttpPost]
        [ActionName("UsersSelectorByIdx")]
        public IActionResult SelectUserByIdx([FromBody] int Idx)
        {
            User result = UsersDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }
    }
}