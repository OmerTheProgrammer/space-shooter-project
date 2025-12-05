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

        // --- EnemiesInLastLevel: ---
        [HttpDelete]
        [ActionName("EnemyInLastLevelDeletor")]
        public IActionResult EnemyInLastLevelDeletor([FromBody] int idx)
        {
            EnemiesInLastLevelDB enemiesInLastLevelDB = new EnemiesInLastLevelDB();
            enemiesInLastLevelDB.Delete(new EnemyInLastLevel { Idx = idx });
            int changedRecords = enemiesInLastLevelDB.SaveChanges();

            if (changedRecords == 0) // Resource not found or already deleted
            {
                // Throw ExpandedException without SQL context. The middleware will catch this
                // and return a 404 Not Found (based on Case A logic in the handler).
                throw new ExpandedException($"Not Found: EnemyInLastLevel with idx = {idx} was not found or has already been deleted.");
            }

            return StatusCode(200, $"OK: Record for EnemyInLastLevel Idx=" +
                $"{idx} was removed.\n" +
                $" Records changed: {changedRecords}");
        }

        // --- GROUPS: ---
        [HttpDelete]
        [ActionName("GroupDeletor")]
        public IActionResult GroupDeletor([FromBody] int idx)
        {
            GroupsDB groupsDB = new GroupsDB();
            groupsDB.Delete(new Group { Idx = idx });
            int changedRecords = groupsDB.SaveChanges();

            if (changedRecords == 0) // Resource not found or already deleted
            {
                // Throw ExpandedException without SQL context. The middleware will catch this
                // and return a 404 Not Found (based on Case A logic in the handler).
                throw new ExpandedException($"Not Found: Group with idx = {idx} was not found or has already been deleted.");
            }

            return StatusCode(200, $"OK: Record for Group Idx=" +
                $"{idx} was removed.\n" +
                $" Records changed: {changedRecords}");
        }

        // --- PLAYERS ---
        [HttpDelete]
        [ActionName("PlayerDeletor")]
        public IActionResult PlayerDeletor([FromBody] int idx)
        {
            PlayersDB playersDB = new PlayersDB();
            playersDB.Delete(new Player { Idx = idx });
            int changedRecords = playersDB.SaveChanges();

            if (changedRecords == 0) // Resource not found or already deleted
            {
                // Throw ExpandedException without SQL context. The middleware will catch this
                // and return a 404 Not Found (based on Case A logic in the handler).
                throw new ExpandedException($"Not Found: Player with idx = {idx} was not found or has already been deleted.");
            }

            return StatusCode(200, $"OK: Record for Player Idx=" +
                $"{idx} was removed.\n" +
                $" Records changed: {changedRecords}");
        }

        // --- ProfileEditRequests ---
        [HttpDelete]
        [ActionName("ProfileEditRequestDeletor")]
        public IActionResult ProfileEditRequestDeletor([FromBody] int idx)
        {
            ProfileEditRequestsDB profileEditRequestsDB = new ProfileEditRequestsDB();
            profileEditRequestsDB.Delete(new ProfileEditRequest { Idx = idx });
            int changedRecords = profileEditRequestsDB.SaveChanges();

            if (changedRecords == 0) // Resource not found or already deleted
            {
                // Throw ExpandedException without SQL context. The middleware will catch this
                // and return a 404 Not Found (based on Case A logic in the handler).
                throw new ExpandedException($"Not Found: ProfileEditRequest with idx = {idx} was not found or has already been deleted.");
            }

            return StatusCode(200, $"OK: Record for ProfileEditRequest Idx=" +
                $"{idx} was removed.\n" +
                $" Records changed: {changedRecords}");
        }

        // --- RequestsData ---
        [HttpDelete]
        [ActionName("RequestDataDeletor")]
        public IActionResult RequestDataDeletor([FromBody] int idx)
        {
            RequestsDataDB requestsDataDB = new RequestsDataDB();
            requestsDataDB.Delete(new RequestData { Idx = idx });
            int changedRecords = requestsDataDB.SaveChanges();

            if (changedRecords == 0) // Resource not found or already deleted
            {
                // Throw ExpandedException without SQL context. The middleware will catch this
                // and return a 404 Not Found (based on Case A logic in the handler).
                throw new ExpandedException($"Not Found: RequestData with idx = {idx} was not found or has already been deleted.");
            }

            return StatusCode(200, $"OK: Record for RequestData Idx=" +
                $"{idx} was removed.\n" +
                $" Records changed: {changedRecords}");
        }

        // --- RunsInfo ---
        [HttpDelete]
        [ActionName("RunInfoDeletor")]
        public IActionResult RunInfoDeletor([FromBody] int idx)
        {
            RunsInfoDB runsInfoDB = new RunsInfoDB();
            runsInfoDB.Delete(new RunInfo { Idx = idx });
            int changedRecords = runsInfoDB.SaveChanges();

            if (changedRecords == 0) // Resource not found or already deleted
            {
                // Throw ExpandedException without SQL context. The middleware will catch this
                // and return a 404 Not Found (based on Case A logic in the handler).
                throw new ExpandedException($"Not Found: RunInfo with idx = {idx} was not found or has already been deleted.");
            }

            return StatusCode(200, $"OK: Record for RunInfo Idx=" +
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