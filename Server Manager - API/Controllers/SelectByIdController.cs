using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Tables;
using ViewModel;

namespace Server_Manager___API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SelectByIdController : Controller
    {
        [HttpPost]
        [ActionName("AdminsSelectorById")]
        public Admin SelectAdminsById([FromBody] int id)
        {
            return AdminsDB.SelectById(id);
        }

        [HttpPost]
        [ActionName("EnemiesInLastLevelSelectorById")]
        public EnemyInLastLevel SelectEnemiesInLastLevelById([FromBody] int id)
        {
            return EnemiesInLastLevelDB.SelectById(id);
        }

        [HttpPost]
        [ActionName("GroupsSelectorById")]
        public Group SelectGroupsById([FromBody] int id)
        {
            return GroupsDB.SelectById(id);
        }

        [HttpPost]
        [ActionName("PlayersSelectorById")]
        public Player SelectPlayersById([FromBody] int id)
        {
            return PlayersDB.SelectById(id);
        }

        [HttpPost]
        [ActionName("ProfileEditRequestsSelectorById")]
        public ProfileEditRequest SelectProfileEditRequestsById([FromBody] int id)
        {
            return ProfileEditRequestsDB.SelectById(id);
        }

        [HttpPost]
        [ActionName("RequestsDataSelectorById")]
        public RequestData SelectRequestsDataById([FromBody] int id)
        {
            return RequestsDataDB.SelectById(id);
        }

        [HttpPost]
        [ActionName("RunsInfoSelectorById")]
        public RunInfo SelectRunsInfoById([FromBody] int id)
        {
            return RunsInfoDB.SelectById(id);
        }

        [HttpPost]
        [ActionName("UsersSelectorById")]
        public User SelectUsersById([FromBody] int id)
        {
            return UsersDB.SelectById(id);
        }
    }
}
