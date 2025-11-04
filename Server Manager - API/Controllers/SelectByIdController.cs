using Microsoft.AspNetCore.Mvc;
using Model.Tables;
using ViewModel;

namespace Server_Manager___API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SelectByIdController : Controller
    {
        [HttpGet]
        [ActionName("AdminsSelectorById")]
        public AdminsTable SelectAllAdmins()
        {
            AdminsDB adminsDB = new AdminsDB();
            AdminsTable adminsTable = adminsDB.SelectAll();
            return adminsTable;
        }

        [HttpGet]
        [ActionName("EnemiesInLastLevelSelector")]
        public EnemiesInLastLevelTable SelectAllEnemiesInLastLevel()
        {
            EnemiesInLastLevelDB EnemiesInLastLevelDB = new EnemiesInLastLevelDB();
            EnemiesInLastLevelTable EnemiesInLastLevelTable = EnemiesInLastLevelDB.SelectAll();
            return EnemiesInLastLevelTable;
        }

        [HttpGet]
        [ActionName("GroupsSelector")]
        public GroupsTable SelectAllGroups()
        {
            GroupsDB GroupsDB = new GroupsDB();
            GroupsTable GroupsTable = GroupsDB.SelectAll();
            return GroupsTable;
        }

        [HttpGet]
        [ActionName("PlayersSelector")]
        public PlayersTable SelectAllPlayers()
        {
            PlayersDB PlayersDB = new PlayersDB();
            PlayersTable PlayersTable = PlayersDB.SelectAll();
            return PlayersTable;
        }

        [HttpGet]
        [ActionName("ProfileEditRequestsSelector")]
        public ProfileEditRequestsTable SelectAllProfileEditRequests()
        {
            ProfileEditRequestsDB ProfileEditRequestsDB = new ProfileEditRequestsDB();
            ProfileEditRequestsTable ProfileEditRequestsTable = ProfileEditRequestsDB.SelectAll();
            return ProfileEditRequestsTable;
        }

        [HttpGet]
        [ActionName("RequestsDataSelector")]
        public RequestsDataTable SelectAllRequestsDataDB()
        {
            RequestsDataDB RequestsDataDB = new RequestsDataDB();
            RequestsDataTable RequestsDataDBTable = RequestsDataDB.SelectAll();
            return RequestsDataDBTable;
        }

        [HttpGet]
        [ActionName("RunsInfoSelector")]
        public RunsInfoTable SelectAllRunsInfo()
        {
            RunsInfoDB RunsInfoDB = new RunsInfoDB();
            RunsInfoTable RunsInfoTable = RunsInfoDB.SelectAll();
            return RunsInfoTable;
        }

        [HttpGet]
        [ActionName("UsersSelector")]
        public UsersTable SelectAllUsers()
        {
            UsersDB UsersDB = new UsersDB();
            UsersTable UsersTable = UsersDB.SelectAll();
            return UsersTable;
        }
    }
}
