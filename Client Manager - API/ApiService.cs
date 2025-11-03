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

        public async Task<AdminsTable> GetAllAdmins()
        {
            return await client.GetFromJsonAsync<AdminsTable>("/api/Insert/CitySelector");
        }

        public async Task<EnemiesInLastLevelTable> GetAllEnemiesInLastLevel()
        {
            return await client.GetFromJsonAsync<EnemiesInLastLevelTable>("/api/SelectAll/EnemiesInLastLevelSelector");
        }

        public async Task<GroupsTable> GetAllGroups()
        {
            return await client.GetFromJsonAsync<GroupsTable>("/api/SelectAll/GroupsSelector");
        }

        public async Task<PlayersTable> GetAllPlayers()
        {
            return await client.GetFromJsonAsync<PlayersTable>("/api/SelectAll/PlayersSelector");
        }

        public async Task<ProfileEditRequestsTable> GetAllProfileEditRequests()
        {
            return await client.GetFromJsonAsync<ProfileEditRequestsTable>("/api/SelectAll/ProfileEditRequestsSelector");
        }

        public async Task<RequestsDataTable> GetAllRequestsDataDB()
        {
            return await client.GetFromJsonAsync<RequestsDataTable>("/api/SelectAll/RequestsDataSelector");
        }

        public async Task<RunsInfoTable> GetAllRunsInfoDB()
        {
            return await client.GetFromJsonAsync<RunsInfoTable>("/api/SelectAll/RunsInfoSelector");
        }

        public async Task<UsersTable> GetAllUsersDB()
        {
            return await client.GetFromJsonAsync<UsersTable>("/api/SelectAll/UsersSelector");
        }
    }
}
