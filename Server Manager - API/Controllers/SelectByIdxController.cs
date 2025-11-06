using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using ViewModel;

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
            try
            {
                Admin result = AdminsDB.SelectByIdx(Idx);
                // If SelectByIdx returns the entity, return HTTP 200 OK.
                return Ok(result);
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

        // --- ENEMIES IN LAST LEVEL ---
        [HttpPost]
        [ActionName("EnemiesInLastLevelSelectorByIdx")]
        public IActionResult SelectEnemiesInLastLevelByIdx([FromBody] int Idx)
        {
            try
            {
                EnemyInLastLevel result = EnemiesInLastLevelDB.SelectByIdx(Idx);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }

        // --- GROUPS ---
        [HttpPost]
        [ActionName("GroupsSelectorByIdx")]
        public IActionResult SelectGroupsByIdx([FromBody] int Idx)
        {
            try
            {
                Group result = GroupsDB.SelectByIdx(Idx);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }

        // --- PLAYERS ---
        [HttpPost]
        [ActionName("PlayersSelectorByIdx")]
        public IActionResult SelectPlayersByIdx([FromBody] int Idx)
        {
            try
            {
                Player result = PlayersDB.SelectByIdx(Idx);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }

        // --- PROFILE EDIT REQUESTS ---
        [HttpPost]
        [ActionName("ProfileEditRequestsSelectorByIdx")]
        public IActionResult SelectProfileEditRequestsByIdx([FromBody] int Idx)
        {
            try
            {
                ProfileEditRequest result = ProfileEditRequestsDB.SelectByIdx(Idx);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }

        // --- REQUESTS DATA ---
        [HttpPost]
        [ActionName("RequestsDataSelectorByIdx")]
        public IActionResult SelectRequestsDataByIdx([FromBody] int Idx)
        {
            try
            {
                RequestData result = RequestsDataDB.SelectByIdx(Idx);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }

        // --- RUN INFO ---
        [HttpPost]
        [ActionName("RunsInfoSelectorByIdx")]
        public IActionResult SelectRunsInfoByIdx([FromBody] int Idx)
        {
            try
            {
                RunInfo result = RunsInfoDB.SelectByIdx(Idx);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }

        // --- USERS ---
        [HttpPost]
        [ActionName("UsersSelectorByIdx")]
        public IActionResult SelectUsersByIdx([FromBody] int Idx)
        {
            try
            {
                User result = UsersDB.SelectByIdx(Idx);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"An unexpected server error occurred: {ex.Message}");
            }
        }
    }
}