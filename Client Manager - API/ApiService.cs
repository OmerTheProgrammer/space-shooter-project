using Model.Entitys;
using Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client_Manager___API
{
    public class ApiService : IApiService
    {
        private string uri;
        private HttpClient client;
        public ApiService(string baseUri)
        {
            uri = baseUri;
            client = new HttpClient();
            client.BaseAddress = new Uri(uri);
        }

        // Default constructor pointing to localhost
        public ApiService() : this("http://localhost:7013") { }

        #region select all:
        private async Task<T> GetTable<T>(string endpoint)
            where T : new()
        {
            try
            {
                // The method uses the relative path defined in the public methods
                return await client.GetFromJsonAsync<T>(endpoint);
            }
            catch (Exception ex)
            {
                // Centralized error logging
                Console.WriteLine($"Error fetching data from {endpoint}: {ex.Message}");
                // Return an empty instance of the table type T
                return new T();
            }
        }

        public async Task<AdminsTable> GetAllAdmins()
        {
            return await GetTable<AdminsTable>("/api/SelectAll/AdminsSelector");
        }

        public async Task<EnemiesInLastLevelTable> GetAllEnemiesInLastLevel()
        {
            return await GetTable<EnemiesInLastLevelTable>("/api/SelectAll/EnemiesInLastLevelSelector");
        }

        public async Task<GroupsTable> GetAllGroups()
        {
            return await GetTable<GroupsTable>("/api/SelectAll/GroupsSelector");
        }

        public async Task<PlayersTable> GetAllPlayers()
        {
            return await GetTable<PlayersTable>("/api/SelectAll/PlayersSelector");
        }

        public async Task<ProfileEditRequestsTable> GetAllProfileEditRequests()
        {
            return await GetTable<ProfileEditRequestsTable>("/api/SelectAll/ProfileEditRequestsSelector");
        }

        public async Task<RequestsDataTable> GetAllRequestsData()
        {
            return await GetTable<RequestsDataTable>("/api/SelectAll/RequestsDataSelector");
        }

        public async Task<RunsInfoTable> GetAllRunsInfo()
        {
            return await GetTable<RunsInfoTable>("/api/SelectAll/RunsInfoSelector");
        }

        public async Task<UsersTable> GetAllUsers()
        {
            return await GetTable<UsersTable>("/api/SelectAll/UsersSelector");
        }
        #endregion

        #region Select by Id
        private async Task<T> GetById<T>(string endpoint, int idx)
            where T : new()
        {
            try
            {
                // The method uses the relative path defined in the public methods
                return await client.GetFromJsonAsync<int>(endpoint, idx);
            }
            catch (Exception ex)
            {
                // Centralized error logging
                Console.WriteLine($"Error fetching data from {endpoint}: {ex.Message}");
                // Return an empty instance of the table type T
                return new T();
            }
        }

        public Task<Admin> GetAdminById(int idx)
        {
            return GetById<Admin>($"/api/SelectByIdController/AdminsSelectorById",idx);
        }

        public Task<EnemyInLastLevel> GetEnemiesInLastLevelById(int idx)
        {
            return GetById<EnemyInLastLevel>($"/api/SelectByIdController/AdminsSelectorById", idx);
        }

        public Task<Group> GetGroupsById(int idx)
        {
            return GetById<Group>($"/api/SelectByIdController/AdminsSelectorById", idx);
        }

        public Task<Player> GetPlayersById(int idx)
        {
            return GetById<Player>($"/api/SelectByIdController/AdminsSelectorById", idx);
        }

        public Task<ProfileEditRequest> GetProfileEditRequestsById(int idx)
        {
            return GetById<ProfileEditRequest>($"/api/SelectByIdController/AdminsSelectorById", idx);
        }

        public Task<RequestData> GetRequestsDataById(int idx)
        {
            return GetById<RequestData>($"/api/SelectByIdController/AdminsSelectorById", idx);
        }

        public Task<RunInfo> GetRunsInfoById(int idx)
        {
            return GetById<RunInfo>($"/api/SelectByIdController/AdminsSelectorById", idx);
        }

        public Task<User> GetUsersById(int idx)
        {
            return GetById<User>($"/api/SelectByIdController/AdminsSelectorById", idx);
        }
        #endregion
    }
}
