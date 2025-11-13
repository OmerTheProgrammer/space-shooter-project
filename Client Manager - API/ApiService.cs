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
        private HttpClient client;
        public ApiService(string baseUri)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
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

                    if (errorContent == "")
                    {
                        errorContent = "The server can't find the requsted service.";
                    }
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
            return GetByIdx<Admin>($"/api/SelectByIdx/AdminsSelectorByIdx", idx);
        }

        public Task<EnemyInLastLevel> GetEnemiesInLastLevelByIdx(int idx)
        {
            return GetByIdx<EnemyInLastLevel>($"/api/SelectByIdx/EnemiesInLastLevelSelectorByIdx", idx);
        }

        public Task<Group> GetGroupsByIdx(int idx)
        {
            return GetByIdx<Group>($"/api/SelectByIdx/GroupsSelectorByIdx", idx);
        }

        public Task<Player> GetPlayersByIdx(int idx)
        {
            return GetByIdx<Player>($"/api/SelectByIdx/PlayersSelectorByIdx", idx);
        }

        public Task<ProfileEditRequest> GetProfileEditRequestsByIdx(int idx)
        {
            return GetByIdx<ProfileEditRequest>($"/api/SelectByIdx/ProfileEditRequestsSelectorByIdx", idx);
        }

        public Task<RequestData> GetRequestsDataByIdx(int idx)
        {
            return GetByIdx<RequestData>($"/api/SelectByIdx/RequestsDataSelectorByIdx", idx);
        }

        public Task<RunInfo> GetRunsInfoByIdx(int idx)
        {
            return GetByIdx<RunInfo>($"/api/SelectByIdx/RunsInfoSelectorByIdx", idx);
        }

        public Task<User> GetUsersByIdx(int idx)
        {
            return GetByIdx<User>($"/api/SelectByIdx/UsersSelectorByIdx", idx);
        }
        #endregion

        #region Insert:
        private async Task<int> Insert<T>(string endpoint, T entity)
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
                // deserialize it into the int
                return int.Parse(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                // Centralized error logging
                Console.WriteLine($"Error fetching data from {endpoint}: {ex.Message}");
                // Return an empty instance of the table type T
                return -1;
            }
        }

        public Task<int> InsertAdmins(Admin admin)
        {
            //returns number of rows affected
            return Insert<Admin>($"/api/Insert/AdminsInsertor", admin);
        }

        public Task<int> InsertEnemiesInLastLevel(EnemyInLastLevel enemyInLastLevel)
        {
            //returns number of rows affected
            return Insert<EnemyInLastLevel>($"/api/Insert/EnemiesInLastLevelInsertor", enemyInLastLevel);
        }

        public Task<int> InsertGroups(Group group)
        {
            //returns number of rows affected
            return Insert<Group>($"/api/Insert/GroupsInsertor", group);
        }

        public Task<int> InsertPlayers(Player player)
        {
            //returns number of rows affected
            return Insert<Player>($"/api/Insert/PlayersInsertor", player);
        }

        public Task<int> InsertProfileEditRequests(ProfileEditRequest profileEditRequest)
        {
            //returns number of rows affected
            return Insert<ProfileEditRequest>($"/api/Insert/ProfileEditRequestsInsertor", profileEditRequest);
        }

        public Task<int> InsertRequestsData(RequestData requestData)
        {
            //returns number of rows affected
            return Insert<RequestData>($"/api/Insert/RequestsDataInsertor", requestData);
        }

        public Task<int> InsertRunsInfo(RunInfo runInfo)
        {
            //returns number of rows affected
            return Insert<RunInfo>($"/api/Insert/RunsInfoInsertor", runInfo);
        }

        public Task<int> InsertUsers(User user)
        {
            //returns number of rows affected
            return Insert<User>($"/api/Insert/UsersInsertor", user);
        }

        #endregion

        #region Update:
        private async Task<int> Update<T>(string endpoint, T entity)
where T : new()
        {
            try
            {
                // 1. Send the PUT request
                HttpResponseMessage response =
                    await client.PutAsJsonAsync(endpoint, entity);

                // 2. Check for success status codes (2xx)
                if (!response.IsSuccessStatusCode)
                {
                    // Read and include the server's specific error message
                    string errorContent = await response.Content.ReadAsStringAsync();

                    // Throw a detailed exception
                    throw new HttpRequestException(
                        $"Request failed: {response.StatusCode} - {errorContent}",
                        null,
                        response.StatusCode
                    );
                }

                // 3. Process success content: Expect a parsable integer (0 or 1)
                string resultString = await response.Content.ReadAsStringAsync();

                if (int.TryParse(resultString.Trim(), out int changedRecords))
                {
                    // Successfully parsed the number of records affected
                    return changedRecords;
                }
                //the server did the thing, but the response.Content failed, assume worked.
                return 1;
            }
            catch (Exception ex)
            {
                // Centralized error logging
                Console.WriteLine($"Error updating data at {endpoint}: {ex.Message}");
                // Return a failure indicator
                return -1;
            }
        }

        public Task<int> UpdateAdmins(Admin admin)
        {
            return Update<Admin>($"/api/Update/AdminUpdator", admin);
        }

        #endregion

        #region Delete:


        public Task<int> DeleteAdmin(int idx)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
