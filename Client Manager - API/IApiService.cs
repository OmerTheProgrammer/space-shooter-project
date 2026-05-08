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
    public interface IApiService
    {
        #region select all:
            public Task<AdminsTable> GetAllAdmins();

            public Task<GroupsTable> GetAllGroups();

            public Task<PlayersAndGroupsTable> GetAllPlayersAndGroups();

            public Task<PlayersTable> GetAllPlayers();

            public Task<ProfileEditRequestsTable> GetAllProfileEditRequests();

            public Task<RequestsDataTable> GetAllRequestsData();

            public Task<RunsInfoTable> GetAllRunsInfo();

            public Task<UsersTable> GetAllUsers();
        #endregion

        #region select by Idx:
            public Task<Admin> GetAdminByIdx(int Idx);

            public Task<Group> GetGroupByIdx(int Idx);

            public Task<PlayerAndGroup> GetPlayerAndGroupByIdx(int Idx);

            public Task<Player> GetPlayerByIdx(int Idx);

            public Task<ProfileEditRequest> GetProfileEditRequestByIdx(int Idx);

            public Task<RequestData> GetRequestDataByIdx(int Idx);

            public Task<RunInfo> GetRunInfoByIdx(int Idx);

            public Task<User> GetUserByIdx(int Idx);
        #endregion

        #region Insert:
            public Task<(int rows, string? error)> InsertAdmin(Admin admin);

            public Task<(int rows, string? error)> InsertGroup(Group group);

            public Task<(int rows, string? error)> InsertPlayerAndGroup(
                PlayerAndGroup playerAndGroup);

            public Task<(int rows, string? error)> InsertPlayer(Player player);

            public Task<(int rows, string? error)> InsertProfileEditRequest(
                ProfileEditRequest profileEditRequest);

            public Task<(int rows, string? error)> InsertRequestData(
                RequestData RequestData);

            public Task<(int rows, string? error)> InsertRunInfo(RunInfo runInfo);

            public Task<(int rows, string? error)> InsertUser(User user);
        #endregion

        #region Update:
            public Task<(int rows, string? error)> UpdateAdmin(AdminDTO admin);

            public Task<(int rows, string? error)> UpdateGroup(GroupDTO group);

            public Task<(int rows, string? error)> UpdatePlayerAndGroup(
                PlayerAndGroupDTO playerAndGroup);

            public Task<(int rows, string? error)> UpdatePlayer(PlayerDTO player);

            public Task<(int rows, string? error)> UpdateProfileEditRequest(
                ProfileEditRequestDTO profileEditRequest);
            
            public Task<(int rows, string? error)> UpdateRequestData(
                RequestDataDTO requestData);
            
            public Task<(int rows, string? error)> UpdateRunInfo(RunInfoDTO runInfo);
            
            public Task<(int rows, string? error)> UpdateUser(UserDTO user);
        #endregion

        #region Delete:
            public Task<(int rows, string? error)> DeleteAdmin(int idx);
            
            public Task<(int rows, string? error)> DeleteGroup(int idx);

            public Task<(int rows, string? error)> DeletePlayerAndGroup(int idx);

            public Task<(int rows, string? error)> DeletePlayer(int idx);

            public Task<(int rows, string? error)> DeleteProfileEditRequest(int idx);

            public Task<(int rows, string? error)> DeleteRequestData(int idx);

            public Task<(int rows, string? error)> DeleteRunInfo(int idx);

            public Task<(int rows, string? error)> DeleteUser(int idx);
        #endregion

        public Task<ConfigSettings> GetAdminKey();
    }
}
