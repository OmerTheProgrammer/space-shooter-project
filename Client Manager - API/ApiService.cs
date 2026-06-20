using Microsoft.Extensions.Configuration;
using Model.Data_Transfer_Objects;
using Model.Entitys;
using Model.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewModel;
using ViewModel.DBs;

namespace Client_Manager___API
{
    public class ApiService : IApiService
    {
        private HttpClient client;
        public ApiService(string baseUri)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            //adds header to all requst to not trigger Microsoft "Anti-Phishing" landing page
            client.DefaultRequestHeaders.Add("X-Tunnel-Skip-AntiPhishing-Scan", "true");
        }

        private static readonly JsonSerializerOptions
            _serializationOptions = new JsonSerializerOptions
            {
                // This is the key change: tells the serializer to skip properties that are null.
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = null
            };

        public ApiService():this(GetServerUrl()) { }
        private static IConfiguration _config;

        private static string GetServerUrl()
        {
            // Load configuration from appsettings.Development.json Use AppContext.BaseDirectory
            // to find the folder where the json is located
            // Climb up to the project root where the .json files actually live
            // 1. Start at ...\Space Shooter Website\Space Shooter Website\bin\Debug\net8.0\
            // 2. Climb 5 levels to reach the 'space-shooter-project' root
            // Level 1: net8.0 -> Debug
            // Level 2: Debug -> bin
            // Level 3: bin -> Inner Project Folder
            // Level 4: Inner Project Folder -> Outer Project Folder
            // Level 5: Outer Project Folder -> Repository Root
            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Client Manager - API"));
            _config = new ConfigurationBuilder()
                .SetBasePath(projectRoot)
                .AddJsonFile("appsettings.json", optional: true) // Standard name
                .AddJsonFile("appsettings.Development.json", optional: true) // Overlay name
                .Build();

            // The tunnel URL from configuration
            string tunnelUrl = _config["ConnectionStrings:SpaceShootersDevTunnel"];
            // Check if the tunnel is reachable by sending a HEAD request with a short timeout from unrlated HttpClient
            using (var client = new HttpClient())
            {
                try
                {
                    // A short timeout for checking responsiveness
                    client.Timeout = TimeSpan.FromSeconds(2);

                    //just checks if the tunnel and server are up
                    var request = new HttpRequestMessage(HttpMethod.Head, tunnelUrl);
                    //the tunnel might have anti-phishing blocking HEAD requests
                    request.Headers.Add("X-Tunnel-Skip-AntiPhishing-Scan", "true");
                    var response = client.Send(request);

                    //are up and responsive?
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(">>> [OK] Tunnel is alive. Using it");
                        return tunnelUrl;
                    }
                }
                catch (TaskCanceledException)
                {
                    // This happens specifically if the 2 seconds ran out
                    Console.WriteLine(">>> [TIMEOUT] Tunnel is slow or server isn't running.");
                }
                catch (Exception ex)
                {
                    // This catches other errors (like no internet or bad URL)
                    Console.WriteLine($">>> [ERROR] Tunnel unreachable: {ex.Message}");
                }
            }

            // This fallback process
            Console.WriteLine(">>> [FALLBACK] Switching to Localhost: ");
            return "https://localhost:7013/";
        }

        #region select all:
        private async Task<T> GetTable<T>(string endpoint)
            where T : class
        {
            try
            {
                // The method uses the relative path defined in the public methods
                return await client.GetFromJsonAsync<T>(endpoint);
            }
            catch (Exception ex)

            {
                //can't throw from here
                // Centralized error logging
                Console.WriteLine($"Error fetching data from {endpoint}: {ex.Message}");
                return null;
            }
        }

        public async Task<AdminsTable> GetAllAdmins()
        {
            return await GetTable<AdminsTable>("/api/SelectAll/AdminsSelector");
        }

        public async Task<GroupsTable> GetAllGroups()
        {
            return await GetTable<GroupsTable>("/api/SelectAll/GroupsSelector");
        }

        public async Task<PlayersAndGroupsTable> GetAllPlayersAndGroups()
        {
            return await GetTable<PlayersAndGroupsTable>("/api/SelectAll/PlayersAndGroupsSelector");
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
            where T : BaseEntity, new()
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
                //can't throw from here
                // Centralized error logging
                Console.WriteLine($"Error fetching data from {endpoint}: {ex.Message}");
                return null;
            }
        }

        public Task<Admin> GetAdminByIdx(int idx)
        {
            return GetByIdx<Admin>($"/api/SelectByIdx/AdminsSelectorByIdx", idx);
        }

        public Task<Model.Entitys.Group> GetGroupByIdx(int idx)
        {
            return GetByIdx<Model.Entitys.Group>($"/api/SelectByIdx/GroupsSelectorByIdx", idx);
        }

        public Task<PlayerAndGroup> GetPlayerAndGroupByIdx(int idx)
        {
            return GetByIdx<PlayerAndGroup>($"/api/SelectByIdx/PlayersAndGroupsSelectorByIdx", idx);
        }

        public Task<Player> GetPlayerByIdx(int idx)
        {
            return GetByIdx<Player>($"/api/SelectByIdx/PlayersSelectorByIdx", idx);
        }

        public Task<ProfileEditRequest> GetProfileEditRequestByIdx(int idx)
        {
            return GetByIdx<ProfileEditRequest>($"/api/SelectByIdx/ProfileEditRequestsSelectorByIdx", idx);
        }

        public Task<RequestData> GetRequestDataByIdx(int idx)
        {
            return GetByIdx<RequestData>($"/api/SelectByIdx/RequestsDataSelectorByIdx", idx);
        }

        public Task<RunInfo> GetRunInfoByIdx(int idx)
        {
            return GetByIdx<RunInfo>($"/api/SelectByIdx/RunsInfoSelectorByIdx", idx);
        }

        public Task<User> GetUserByIdx(int idx)
        {
            return GetByIdx<User>($"/api/SelectByIdx/UsersSelectorByIdx", idx);
        }
        #endregion

        #region Insert:
        ///<summary>
        /// Generic Update method that sends a PUT request with the Entity.
        /// returns the number of affected rows.
        ///</summary>
        private async Task<(int rows, string? error)> Insert<T>(string endpoint, T entity)
            where T : new()
        {
            int changedRecords = -1;
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

                    //reduce clutter for user
                    // 3. Throw a detailed exception that includes the server's message.
                    throw new HttpRequestException(
                        /*$"Request failed: {response.StatusCode} - " + */ $"{errorContent}",
                        null,
                        response.StatusCode
                    );

                }
                // 3. Read the JSON content as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                // convert it into the int
                if (int.TryParse(responseContent, out changedRecords))
                {
                    return (changedRecords, null);
                }
                else
                {
                    throw new HttpRequestException(
                        $"Parsing Error: Server response for INSERT was not " +
                        $"a valid integer: '{responseContent}'.",
                        null
                    );
                }
            }
            catch (Exception ex)
            {
                //can't throw from here
                // Centralized error logging
                Console.WriteLine($"Error inserting from {endpoint}: {ex.Message}");
                //clutter for user hidden
                return (changedRecords, /*"Error inserting from {endpoint}:" + */ $"{ex.Message}");
            }
        }

        public Task<(int rows, string? error)> InsertAdmin(Admin admin)
        {
            //returns number of rows affected
            return Insert<Admin>($"/api/Insert/AdminsInsertor", admin);
        }

        public Task<(int rows, string? error)> InsertGroup(Model.Entitys.Group group)
        {
            //returns number of rows affected
            return Insert<Model.Entitys.Group>($"/api/Insert/GroupsInsertor", group);
        }

        public Task<(int rows, string? error)> InsertPlayerAndGroup(PlayerAndGroup playerAndGroup)
        {
            //returns number of rows affected
            return Insert<PlayerAndGroup>($"/api/Insert/PlayersAndGroupsInsertor", playerAndGroup);
        }

        public Task<(int rows, string? error)> InsertPlayer(Player player)
        {
            //returns number of rows affected
            return Insert<Player>($"/api/Insert/PlayersInsertor", player);
        }

        public Task<(int rows, string? error)> InsertProfileEditRequest(ProfileEditRequest profileEditRequest)
        {
            //returns number of rows affected
            return Insert<ProfileEditRequest>($"/api/Insert/ProfileEditRequestsInsertor", profileEditRequest);
        }

        public Task<(int rows, string? error)> InsertRequestData(RequestData RequestData)
        {
            //returns number of rows affected
            return Insert<RequestData>($"/api/Insert/RequestsDataInsertor", RequestData);
        }

        public Task<(int rows, string? error)> InsertRunInfo(RunInfo runInfo)
        {
            //returns number of rows affected
            return Insert<RunInfo>($"/api/Insert/RunsInfoInsertor", runInfo);
        }

        public Task<(int rows, string? error)> InsertUser(User user)
        {
            //returns number of rows affected
            return Insert<User>($"/api/Insert/UsersInsertor", user);
        }
        #endregion

        #region Update:
        ///<summary>
        /// Generic Update method that sends a PUT request with the Entity.
        /// returns the number of affected rows.
        ///</summary>
        private async Task<(int rows, string? error)> Update<T>(string endpoint, T entity)
            where T : new()
        {
            int changedRecords = -1;
            try
            {
                // 1. becouse nulls are possible, we need to create the content manually
                // becouse PutAsJsonAsync does not support options parameters
                string jsonContent = JsonSerializer.Serialize(entity, _serializationOptions);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                // 1. Send the PUT request not using PutAsJsonAsync becouse
                // content the json is now manually created
                HttpResponseMessage response =
                        await client.PutAsync(endpoint, content);

                // 2. Check for success status codes (2xx)
                if (!response.IsSuccessStatusCode)
                {
                    // Read and include the server's specific error message
                    string errorContent = await response.Content.ReadAsStringAsync();

                    //reduce clutter for user
                    // Throw a detailed exception
                    throw new HttpRequestException(
                        /*$"Request failed: {response.StatusCode} -" +*/ $" {errorContent}",
                        null,
                        response.StatusCode
                    );
                }

                // 3. Process success content: Expect a parsable integer (0 or 1)
                string resultString = await response.Content.ReadAsStringAsync();

                var regex = new Regex(@"Records changed: (\d+)");
                var match = regex.Match(resultString);

                if (!(match.Success && int.TryParse(match.Groups[1].Value,
                    out changedRecords)))
                {
                    throw new HttpRequestException(
                        $"Parsing Error: Server response for UPDATE was not in" +
                        $" the expected format: '{resultString}'.",
                        null
                    );
                }
                // Successfully parsed the number of records affected
                return (changedRecords, null);

            }
            catch (Exception ex)
            {
                //can't throw from here
                //error logging
                Console.WriteLine($"Error updating from {endpoint}: {ex.Message}");
                //clutter for user hidden
                return (changedRecords, /*$"Error updating from {endpoint}:" +*/ $"{ex.Message}");
            }
        }

        public Task<(int rows, string? error)> UpdateAdmin(AdminDTO admin)
        {
            return Update<AdminDTO>($"/api/Update/AdminUpdator", admin);
        }

        public Task<(int rows, string? error)> UpdateGroup(GroupDTO group)
        {
            return Update<GroupDTO>($"/api/Update/GroupUpdator", group);
        }

        public Task<(int rows, string? error)> UpdatePlayerAndGroup(PlayerAndGroupDTO playerAndGroup)
        {
            return Update<PlayerAndGroupDTO>($"/api/Update/PlayerAndGroupUpdator", playerAndGroup);
        }

        public Task<(int rows, string? error)> UpdatePlayer(PlayerDTO player)
        {
            return Update<PlayerDTO>($"/api/Update/PlayerUpdator", player);
        }

        public Task<(int rows, string? error)> UpdateProfileEditRequest(ProfileEditRequestDTO profileEditRequest)
        {
            return Update<ProfileEditRequestDTO>($"/api/Update/ProfileEditRequestUpdator", profileEditRequest);
        }

        public Task<(int rows, string? error)> UpdateRequestData(RequestDataDTO requestData)
        {
            return Update<RequestDataDTO>($"/api/Update/RequestDataUpdator", requestData);
        }

        public Task<(int rows, string? error)> UpdateRunInfo(RunInfoDTO runInfo)
        {
            return Update<RunInfoDTO>($"/api/Update/RunInfoUpdator", runInfo);
        }

        public Task<(int rows, string? error)> UpdateUser(UserDTO user)
        {
            return Update<UserDTO>($"/api/Update/UserUpdator", user);
        }
        #endregion

        #region Delete:
        ///<summary>
        /// Generic delete method that sends a DELETE request with the idx in the body.
        /// returns the number of affected rows
        ///</summary>
        private async Task<(int rows, string? error)> Delete(string endpoint, int idx)
        {
            int changedRecords = -1;
            try
            {
                //basic DELETE requst does not support body, where I hid the idx,
                //so we need to create the request manually
                var request = new HttpRequestMessage(HttpMethod.Delete, endpoint)
                {
                    // Use JsonContent.Create to serialize the integer ID into the body
                    Content = JsonContent.Create(idx)
                };

                HttpResponseMessage response = await client.SendAsync(request);

                // 2. Check for failure status codes
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();

                    //reduce clutter for user
                    throw new HttpRequestException(
                        /*$"Request failed: {response.StatusCode} - " + */$"{errorContent}",
                        null,
                        response.StatusCode
                    );
                }

                // 3. Read the response string (which contains the changed records count)
                string responseContent = await response.Content.ReadAsStringAsync();

                // The server returns a verbose string (e.g., "... Records changed: 1"). 
                // We need to reliably extract the number of affected rows.

                // Find "Records changed:"
                int index = responseContent.IndexOf("Records changed:");
                if (index != -1)
                {
                    string numberPart = responseContent.Substring(index + "Records changed:".Length).Trim();
                    // Try to parse the number
                    if (int.TryParse(numberPart, out changedRecords))
                    {
                        return (changedRecords, null);
                    }
                    else
                    {
                        throw new HttpRequestException(
                            $"Parsing Error: Server response for DELETE was not in the expected format: '{responseContent}'.",
                            null
                        );
                    }
                }
                throw new HttpRequestException(
                    $"not found in: {idx}.",
                    null
                );
            }
            catch (Exception ex)
            {
                //can't throw from here
                // Centralized error logging
                Console.WriteLine($"Error deleting from {endpoint}: {ex.Message}");
                //clutter for user hidden
                return (changedRecords, /*$"Error deleting from {endpoint}:" + */ $"{ex.Message}");
            }
        }

        public Task<(int rows, string? error)> DeleteAdmin(int idx)
        {
            // returns number of rows affected
            return Delete($"/api/Delete/AdminDeletor", idx);
        }

        public Task<(int rows, string? error)> DeleteGroup(int idx)
        {
            // returns number of rows affected
            return Delete($"/api/Delete/GroupDeletor", idx);
        }

        public Task<(int rows, string? error)> DeletePlayerAndGroup(int idx)
        {
            // returns number of rows affected
            return Delete($"/api/Delete/PlayerAndGroupDeletor", idx);
        }

        public Task<(int rows, string? error)> DeletePlayer(int idx)
        {
            // returns number of rows affected
            return Delete($"/api/Delete/PlayerDeletor", idx);
        }

        public Task<(int rows, string? error)> DeleteProfileEditRequest(int idx)
        {
            // returns number of rows affected
            return Delete($"/api/Delete/ProfileEditRequestDeletor", idx);
        }

        public Task<(int rows, string? error)> DeleteRequestData(int idx)
        {
            // returns number of rows affected
            return Delete($"/api/Delete/RequestDataDeletor", idx);
        }

        public Task<(int rows, string? error)> DeleteRunInfo(int idx)
        {
            // returns number of rows affected
            return Delete($"/api/Delete/RunInfoDeletor", idx);
        }

        public Task<(int rows, string? error)> DeleteUser(int idx)
        {
            // returns number of rows affected
            return Delete($"/api/Delete/UserDeletor", idx);
        }
        #endregion

        public async Task<ConfigSettings> GetAdminKey()
        {
            return new ConfigSettings() {
                AdminKey = TextHasher.Hash(_config["AdminSettings:AdminKey"] ?? "")
            };
        }
    }
}
