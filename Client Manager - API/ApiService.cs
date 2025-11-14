using Model.Data_Transfer_Objects;
using Model.Entitys;
using Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
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

        private static readonly JsonSerializerOptions
            _serializationOptions = new JsonSerializerOptions
            {
                // This is the key change: tells the serializer to skip properties that are null.
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = null
            };

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

            public Task<Model.Entitys.Group> GetGroupsByIdx(int idx)
            {
                return GetByIdx<Model.Entitys.Group>($"/api/SelectByIdx/GroupsSelectorByIdx", idx);
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
            ///<summary>
            /// Generic Update method that sends a PUT request with the Entity.
            /// returns the number of affected rows.
            ///</summary>
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
                    // 3. Read the JSON content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // convert it into the int
                    if (int.TryParse(responseContent, out int newId))
                    {
                        return newId;
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
                    // Centralized error logging
                    throw new HttpRequestException(
                        $"Error inserting data to {endpoint}: {ex.Message}",
                        ex
                    );
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

            public Task<int> InsertGroups(Model.Entitys.Group group)
            {
                //returns number of rows affected
                return Insert<Model.Entitys.Group>($"/api/Insert/GroupsInsertor", group);
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
            ///<summary>
            /// Generic Update method that sends a PUT request with the Entity.
            /// returns the number of affected rows.
            ///</summary>
            private async Task<int> Update<T>(string endpoint, T entity)
                where T : new()
            {
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

                        // Throw a detailed exception
                        throw new HttpRequestException(
                            $"Request failed: {response.StatusCode} - {errorContent}",
                            null,
                            response.StatusCode
                        );
                    }

                    // 3. Process success content: Expect a parsable integer (0 or 1)
                    string resultString = await response.Content.ReadAsStringAsync();

                    var regex = new Regex(@"Records changed: (\d+)");
                    var match = regex.Match(resultString);

                    if (!(match.Success && int.TryParse(match.Groups[1].Value, out int changedRecords)))
                    {
                        throw new HttpRequestException(
                            $"Parsing Error: Server response for UPDATE was not in" +
                            $" the expected format: '{resultString}'.",
                            null
                        );
                    }
                    // Successfully parsed the number of records affected
                    return changedRecords;

                }
                catch (Exception ex)
                {
                    //error logging
                    throw new HttpRequestException(
                        $"Error updating data at {endpoint}: {ex.Message}",
                        ex
                    );
                }
            }

            public Task<int> UpdateAdmins(AdminDTO admin)
            {
                return Update<AdminDTO>($"/api/Update/AdminUpdator", admin);
            }
            
            
        #endregion

        #region Delete:
            ///<summary>
            /// Generic delete method that sends a DELETE request with the idx in the body.
            /// returns the number of affected rows
            ///</summary>
            private async Task<int> Delete(string endpoint, int idx)
            {
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

                        throw new HttpRequestException(
                            $"Request failed: {response.StatusCode} - {errorContent}",
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
                        if (int.TryParse(numberPart, out int changedRecords))
                        {
                            return changedRecords;
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
                    // Centralized error logging
                    Console.WriteLine($"Error deleting data via {endpoint}: {ex.Message}");
                    return -1;
                }
            }

            public Task<int> DeleteAdmin(int idx)
            {
                // returns number of rows affected
                return Delete($"/api/Delete/AdminDeletor", idx);
            }
            
            
        #endregion
    }
}
