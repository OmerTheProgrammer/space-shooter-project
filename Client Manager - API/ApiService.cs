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

        #region Select by Idx
        private async Task<T> GetByIdx<T>(string endpoint, int idx)
            where T : new()
        {
            try
            {
                // The method uses the relative path defined in the public methods
                HttpResponseMessage response = 
                    await client.PostAsJsonAsync(endpoint, idx);
                // 2. Ensure the request was successful, if not tell the client about the failure
                if (!response.IsSuccessStatusCode)
                {
                    // 2. Read the specific error content from the server
                    // This reads the body containing the server's error message (e.g., "Idx not found")
                    string errorContent = await response.Content.ReadAsStringAsync();

                    // 3. Throw a detailed exception that includes the server's message.
                    throw new HttpRequestException(
                        $"Request failed: {response.StatusCode} - {errorContent}",
                        null,
                        response.StatusCode
                    );
                }
                // 3. Read the JSON content and
                // deserialize it into the target type <T>
                T result = await response.Content.ReadFromJsonAsync<T>();
                return result;
            }
            catch (Exception ex)
            {
                // Centralized error logging
                Console.WriteLine($"Error fetching data from {endpoint}: {ex.Message}");
                // Return an empty instance of the table type T
                return new T();
            }
        }

        public Task<Admin> GetAdminsByIdx(int idx)
        {
            return GetByIdx<Admin>($"/api/SelectByIdx/AdminsSelectorById",idx);
        }

        public Task<EnemyInLastLevel> GetEnemiesInLastLevelByIdx(int idx)
        {
            return GetByIdx<EnemyInLastLevel>($"/api/SelectByIdx/EnemiesInLastLevelSelectorById", idx);
        }

        public Task<Group> GetGroupsByIdx(int idx)
        {
            return  GetByIdx<Group>($"/api/SelectByIdx/GroupsSelectorById", idx);
        }

        public Task<Player> GetPlayersByIdx(int idx)
        {
            return GetByIdx<Player>($"/api/SelectByIdx/PlayersSelectorById", idx);
        }

        public Task<ProfileEditRequest> GetProfileEditRequestsByIdx(int idx)
        {
            return GetByIdx<ProfileEditRequest>($"/api/SelectByIdx/ProfileEditRequestsSelectorById", idx);
        }

        public Task<RequestData> GetRequestsDataByIdx(int idx)
        {
            return GetByIdx<RequestData>($"/api/SelectByIdx/RequestsDataSelectorById", idx);
        }

        public Task<RunInfo> GetRunsInfoByIdx(int idx)
        {
            return GetByIdx<RunInfo>($"/api/SelectByIdx/RunsInfoSelectorById", idx);
        }

        public Task<User> GetUsersByIdx(int idx)
        {
            return GetByIdx<User>($"/api/SelectByIdx/UsersSelectorById", idx);
        }
        #endregion

        #region Insert:
        private async Task<T> Insert<T>(string endpoint, T entity)
            where T : new()
        {
            try
            {
                // The method uses the relative path defined in the public methods
                HttpResponseMessage response =
                    await client.PostAsJsonAsync(endpoint, entity);
                // 2. Ensure the request was successful, if not tell the client about the failure
                if (!response.IsSuccessStatusCode)
                {
                    // 2. Read the specific error content from the server
                    // This reads the body containing the server's error message (e.g., "Idx not found")
                    string errorContent = await response.Content.ReadAsStringAsync();

                    // 3. Throw a detailed exception that includes the server's message.
                    throw new HttpRequestException(
                        $"Request failed: {response.StatusCode} - {errorContent}",
                        null,
                        response.StatusCode
                    );
                }
                // 3. Read the JSON content and
                // deserialize it into the target type <T>
                T result = await response.Content.ReadFromJsonAsync<T>();
                return result;
            }
            catch (Exception ex)
            {
                // Centralized error logging
                Console.WriteLine($"Error fetching data from {endpoint}: {ex.Message}");
                // Return an empty instance of the table type T
                return new T();
            }
        }

        public Task<Admin> InsertAdmins(Admin admin)
        {
            return Insert<Admin>($"/api/Insert/AdminsInsertor", admin);
        }


        #endregion
    }
}
