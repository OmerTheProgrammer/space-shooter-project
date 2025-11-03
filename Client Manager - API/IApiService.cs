using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Tables;


namespace Client_Manager___API
{
    internal interface IApiService
    {
        public Task<AdminsTable> GetAllAdmins();
        
        public Task<EnemiesInLastLevelTable> GetAllEnemiesInLastLevel();

        public Task<GroupsTable> GetAllGroups();

        public Task<PlayersTable> GetAllPlayers();

        public Task<ProfileEditRequestsTable> GetAllProfileEditRequests();

        public Task<RequestsDataTable> GetAllRequestsDataDB();

        public Task<RunsInfoTable> GetAllRunsInfoDB();

        public Task<UsersTable> GetAllUsersDB();
    }
}
