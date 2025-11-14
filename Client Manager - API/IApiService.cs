using Model.Data_Transfer_Objects;
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

        #region select by Idx:
            public Task<Admin> GetAdminsByIdx(int Idx);

            public Task<EnemyInLastLevel> GetEnemiesInLastLevelByIdx(int Idx);

            public Task<Group> GetGroupsByIdx(int Idx);

            public Task<Player> GetPlayersByIdx(int Idx);

            public Task<ProfileEditRequest> GetProfileEditRequestsByIdx(int Idx);

            public Task<RequestData> GetRequestsDataByIdx(int Idx);

            public Task<RunInfo> GetRunsInfoByIdx(int Idx);

            public Task<User> GetUsersByIdx(int Idx);
        #endregion

        #region Insert:
            public Task<int> InsertAdmins(Admin admin);

            public Task<int> InsertEnemiesInLastLevel(EnemyInLastLevel admin);

            public Task<int> InsertGroups(Group group);

            public Task<int> InsertPlayers(Player player);

            public Task<int> InsertProfileEditRequests(ProfileEditRequest profileEditRequest);

            public Task<int> InsertRequestsData(RequestData requestData);

            public Task<int> InsertRunsInfo(RunInfo runInfo);

            public Task<int> InsertUsers(User user);
        #endregion

        #region Update:
            public Task<int> UpdateAdmins(AdminDTO admin);
            
            
        #endregion

        #region Delete:
            public Task<int> DeleteAdmin(int idx);

        #endregion
    }
}
