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
        public IActionResult SelectAdminsByIdx([FromBody] int Idx)
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
        public IActionResult SelectEnemiesInLastLevelByIdx([FromBody] int Idx)
        {
            EnemyInLastLevel result = EnemiesInLastLevelDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- GROUPS ---
        [HttpPost]
        [ActionName("GroupsSelectorByIdx")]
        public IActionResult SelectGroupsByIdx([FromBody] int Idx)
        {
            Group result = GroupsDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- PLAYERS ---
        [HttpPost]
        [ActionName("PlayersSelectorByIdx")]
        public IActionResult SelectPlayersByIdx([FromBody] int Idx)
        {
            Player result = PlayersDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- PROFILE EDIT REQUESTS ---
        [HttpPost]
        [ActionName("ProfileEditRequestsSelectorByIdx")]
        public IActionResult SelectProfileEditRequestsByIdx([FromBody] int Idx)
        {
            ProfileEditRequest result = ProfileEditRequestsDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- REQUESTS DATA ---
        [HttpPost]
        [ActionName("RequestsDataSelectorByIdx")]
        public IActionResult SelectRequestsDataByIdx([FromBody] int Idx)
        {
            RequestingData result = RequestsDataDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- RUN INFO ---
        [HttpPost]
        [ActionName("RunsInfoSelectorByIdx")]
        public IActionResult SelectRunsInfoByIdx([FromBody] int Idx)
        {
            RunInfo result = RunsInfoDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }

        // --- USERS ---
        [HttpPost]
        [ActionName("UsersSelectorByIdx")]
        public IActionResult SelectUsersByIdx([FromBody] int Idx)
        {
            User result = UsersDB.SelectByIdx(Idx);
            return StatusCode(200, result);
        }
    }
}