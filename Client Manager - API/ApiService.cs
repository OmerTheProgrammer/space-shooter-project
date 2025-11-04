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
        private async Task<T> GetTableAsync<T>(string endpoint)
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
            return await GetTableAsync<AdminsTable>("/api/SelectAll/AdminsSelector");
        }

        public async Task<EnemiesInLastLevelTable> GetAllEnemiesInLastLevel()
        {
            return await GetTableAsync<EnemiesInLastLevelTable>("/api/SelectAll/EnemiesInLastLevelSelector");
        }

        public async Task<GroupsTable> GetAllGroups()
        {
            return await GetTableAsync<GroupsTable>("/api/SelectAll/GroupsSelector");
        }

        public async Task<PlayersTable> GetAllPlayers()
        {
            return await GetTableAsync<PlayersTable>("/api/SelectAll/PlayersSelector");
        }

        public async Task<ProfileEditRequestsTable> GetAllProfileEditRequests()
        {
            return await GetTableAsync<ProfileEditRequestsTable>("/api/SelectAll/ProfileEditRequestsSelector");
        }

        public async Task<RequestsDataTable> GetAllRequestsDataDB()
        {
            return await GetTableAsync<RequestsDataTable>("/api/SelectAll/RequestsDataSelector");
        }

        public async Task<RunsInfoTable> GetAllRunsInfoDB()
        {
            return await GetTableAsync<RunsInfoTable>("/api/SelectAll/RunsInfoSelector");
        }

        public async Task<UsersTable> GetAllUsersDB()
        {
            return await GetTableAsync<UsersTable>("/api/SelectAll/UsersSelector");
        }
        #endregion

        #region Select by Id

        #endregion
    }
}
