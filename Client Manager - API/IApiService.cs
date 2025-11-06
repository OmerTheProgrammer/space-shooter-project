using Model.Entitys;
using Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Client_Manager___API
{
    internal interface IApiService
    {
        #region select all:
        public Task<AdminsTable> GetAllAdmins();
        
        public Task<EnemiesInLastLevelTable> GetAllEnemiesInLastLevel();

        public Task<GroupsTable> GetAllGroups();

        public Task<PlayersTable> GetAllPlayers();

        public Task<ProfileEditRequestsTable> GetAllProfileEditRequests();

        public Task<RequestsDataTable> GetAllRequestsData();

        public Task<RunsInfoTable> GetAllRunsInfo();

        public Task<UsersTable> GetAllUsers();
        #endregion

        #region select by id:
        public Task<Admin> GetAdminById(int idx);

        public Task<EnemyInLastLevel> GetEnemiesInLastLevelById(int idx);

        public Task<Group> GetGroupsById(int idx);

        public Task<Player> GetPlayersById(int idx);

        public Task<ProfileEditRequest> GetProfileEditRequestsById(int idx);

        public Task<RequestData> GetRequestsDataById(int idx);

        public Task<RunInfo> GetRunsInfoById(int idx);

        public Task<User> GetUsersById(int idx);
        #endregion


    }
}
