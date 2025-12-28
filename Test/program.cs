using Client_Manager___API;
using Model;
using Model.Data_Transfer_Objects;
using Model.Entitys;
using Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using ViewModel.DBs;

namespace Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //added unique debug mode to this project only and added to it:
            //RUNNING_TEST_SERVER = true,
            //then changed startup projects, the test with server to run the
            //new debug mode, with the EnvironmentVariable.
            if (Environment.GetEnvironmentVariable("RUNNING_TEST_SERVER") == "true")
            {
                Console.WriteLine("ServerFull mode activated: API Test.");
                ServerFullMain();
            }
            else
            {
                Console.WriteLine("ServerLess mode activated: Local DB Test.");
                ServerLessMain();
                Console.WriteLine("done!");
            }
            Console.ReadLine();

        }

        public static void ServerLessMain()
        {
            #region users
            //UsersDB UserDB = new UsersDB();
            //UsersTable ut = UserDB.SelectAll();
            //Console.WriteLine("users: ");
            //foreach (var t in ut)
            //{
            //    Console.WriteLine(t);
            //}
            //Console.WriteLine();

            //User user = new User() { Id = "122431414" };
            //UserDB.Insert(user);
            //Console.WriteLine($"{UserDB.SaveChanges()} affected rows.");
            //ut = UserDB.SelectAll();
            //foreach (var t in ut)
            //{
            //    Console.WriteLine(t);
            //}
            //Console.WriteLine();

            //user = ut.Last();
            //user.Username = "user245";
            //UserDB.Update(user);
            //Console.WriteLine($"{UserDB.SaveChanges()} affected rows.");
            //ut = UserDB.SelectAll();
            //foreach (var t in ut)
            //{
            //    Console.WriteLine(t);
            //}
            //Console.WriteLine();

            //UserDB.Delete(ut.Last());
            //Console.WriteLine($"{UserDB.SaveChanges()} affected rows.");
            //ut = UserDB.SelectAll();
            //foreach (var t in ut)
            //{
            //    Console.WriteLine(t);
            //}
            //Console.WriteLine();
            #endregion

            #region players
            //Console.WriteLine("players: ");
            //PlayersDB PlayerDB = new PlayersDB();
            //PlayersTable pt = PlayerDB.SelectAll();
            //foreach (var item in pt)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();

            //Player Player = new Player() { Id = "454252552" };
            //PlayerDB.Insert(Player);
            //Console.WriteLine($"{PlayerDB.SaveChanges()} affected rows.");
            //pt = PlayerDB.SelectAll();
            //Console.WriteLine(pt.Last());
            //Console.WriteLine();

            //Player = pt.Last();
            //Player.Username = "Player245";
            //PlayerDB.Update(Player);
            //Console.WriteLine($"{PlayerDB.SaveChanges()} affected rows.");
            //pt = PlayerDB.SelectAll();
            //Console.WriteLine(pt.Last());
            //Console.WriteLine();

            //PlayerDB.Delete(pt.Last());
            //Console.WriteLine($"{PlayerDB.SaveChanges()} affected rows.");
            //pt = PlayerDB.SelectAll();
            //foreach (var t in pt)
            //{
            //    Console.WriteLine(t);
            //}
            //Console.WriteLine();
            #endregion

            #region admins
            //AdminsDB AdminDB = new AdminsDB();
            //AdminsTable at = AdminDB.SelectAll();
            //Console.WriteLine("admins: ");
            //foreach (var item in at)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();

            //Admin Admin = new Admin() { Id = "74746747" };
            //AdminDB.Insert(Admin);
            //Console.WriteLine($"{AdminDB.SaveChanges()} affected rows.");
            //at = AdminDB.SelectAll();
            //Console.WriteLine(at.Last());
            //Console.WriteLine();

            //Admin = at.Last();
            //Admin.Username = "Admin245";
            //AdminDB.Update(Admin);
            //Console.WriteLine($"{AdminDB.SaveChanges()} affected rows.");
            //at = AdminDB.SelectAll();
            //Console.WriteLine(at.Last());
            //Console.WriteLine();

            //AdminDB.Delete(at.Last());
            //Console.WriteLine($"{AdminDB.SaveChanges()} affected rows.");
            //at = AdminDB.SelectAll();
            //foreach (var t in at)
            //{
            //    Console.WriteLine(t);
            //}
            //Console.WriteLine();
            #endregion

            #region ProfileEditRequests
            //ProfileEditRequestsDB ProfileEditRequestingDB = new ProfileEditRequestsDB();
            //ProfileEditRequestsTable ProfTbl = ProfileEditRequestingDB.SelectAll();
            //Console.WriteLine("ProfileEditRequests: ");
            //foreach (var item in ProfTbl)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();

            //ProfileEditRequest ProfileEditRequest = new ProfileEditRequest() { RequestingPlayer = new Player() { Idx = 3 }, Status = Status.Pending };
            //ProfileEditRequestingDB.Insert(ProfileEditRequest);
            //Console.WriteLine($"{ProfileEditRequestingDB.SaveChanges()} affected rows.");
            //ProfTbl = ProfileEditRequestingDB.SelectAll();
            //Console.WriteLine(ProfTbl.Last());
            //Console.WriteLine();

            //ProfileEditRequest = ProfTbl.Last();
            //ProfileEditRequest.Status = Status.Approved;
            //ProfileEditRequestingDB.Update(ProfileEditRequest);
            //Console.WriteLine($"{ProfileEditRequestingDB.SaveChanges()} affected rows.");
            //ProfTbl = ProfileEditRequestingDB.SelectAll();
            //Console.WriteLine(ProfTbl.Last());
            //Console.WriteLine();

            //ProfileEditRequestingDB.Delete(ProfTbl.Last());
            //Console.WriteLine($"{ProfileEditRequestingDB.SaveChanges()} affected rows.");
            //ProfTbl = ProfileEditRequestingDB.SelectAll();
            //Console.WriteLine(ProfTbl.Last());
            //Console.WriteLine();
            #endregion

            #region RequestsData
            //RequestsDataDB RequestsDataDB = new RequestsDataDB();
            //RequestsDataTable rt = RequestsDataDB.SelectAll();
            //Console.WriteLine("RequestsData: ");
            //foreach (var item in rt)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();

            //RequestData RequestsData = new RequestData()
            //{
            //    Request = new ProfileEditRequest { Idx = 1 },
            //    Field = "3654"
            //};
            //RequestsDataDB.Insert(RequestsData);
            //Console.WriteLine($"{RequestsDataDB.SaveChanges()} affected rows.");

            //rt = RequestsDataDB.SelectAll();
            //Console.WriteLine(rt.Last());
            //Console.WriteLine();

            //RequestsData = rt.Last();
            //RequestsData.NewValue = "111";
            //RequestsDataDB.Update(RequestsData);
            //Console.WriteLine($"{RequestsDataDB.SaveChanges()} affected rows.");
            //rt = RequestsDataDB.SelectAll();
            //Console.WriteLine(rt.Last());
            //Console.WriteLine();

            //RequestsDataDB.Delete(rt.Last());
            //Console.WriteLine($"{RequestsDataDB.SaveChanges()} affected rows.");
            //rt = RequestsDataDB.SelectAll();
            //foreach (var t in rt)
            //{
            //    Console.WriteLine(t);
            //}
            //Console.WriteLine();
            #endregion

            #region Groups
            //GroupsDB GroupDB = new GroupsDB();
            //GroupsTable gt = GroupDB.SelectAll();
            //Console.WriteLine("Groups: ");
            //foreach (var item in gt)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();

            //Group group = new Group() { Score = 1 };
            //GroupDB.Insert(group);
            //Console.WriteLine($"{GroupDB.SaveChanges()} affected rows.");
            //gt = GroupDB.SelectAll();
            //Console.WriteLine(gt.Last());
            //Console.WriteLine();

            //group = gt.Last();
            //group.Score += 100;
            //GroupDB.Update(group);
            //Console.WriteLine($"{GroupDB.SaveChanges()} affected rows.");
            //gt = GroupDB.SelectAll();
            //Console.WriteLine(gt.Last());
            //Console.WriteLine();

            //GroupDB.Delete(gt.Last());
            //Console.WriteLine($"{GroupDB.SaveChanges()} affected rows.");
            //gt = GroupDB.SelectAll();
            //Console.WriteLine(gt.Last());
            //Console.WriteLine();
            #endregion

            #region RunsInfo
            //RunsInfoDB RunInfoDB = new RunsInfoDB();
            //RunsInfoTable RunTbl = RunInfoDB.SelectAll();
            //Console.WriteLine("RunsInfo: ");
            //foreach (var item in RunTbl)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();

            //RunInfo RunInfo = new RunInfo()
            //{
            //    Player =
            //new Player { Idx = 3 },
            //    CurrentLevel = 1
            //};
            //RunInfoDB.Insert(RunInfo);
            //Console.WriteLine($"{RunInfoDB.SaveChanges()} affected rows.");
            //RunTbl = RunInfoDB.SelectAll();
            //Console.WriteLine(RunTbl.Last());
            //Console.WriteLine();

            //RunInfo = RunTbl.Last();
            //RunInfo.CurrentLevel += 5;
            //RunInfoDB.Update(RunInfo);
            //Console.WriteLine($"{RunInfoDB.SaveChanges()} affected rows.");
            //RunTbl = RunInfoDB.SelectAll();
            //Console.WriteLine(RunTbl.Last());
            //Console.WriteLine();

            //RunInfoDB.Delete(RunTbl.Last());
            //Console.WriteLine($"{RunInfoDB.SaveChanges()} affected rows.");
            //RunTbl = RunInfoDB.SelectAll();
            //Console.WriteLine(RunTbl.Last());
            //Console.WriteLine();
            #endregion

            #region EnemiesInLastLevel
            //EnemiesInLastLevelDB EnemyInLastLevelDB = new EnemiesInLastLevelDB();
            //EnemiesInLastLevelTable et = EnemyInLastLevelDB.SelectAll();
            //Console.WriteLine("EnemiesInLastLevel: ");
            //foreach (var item in et)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();

            //EnemyInLastLevel EnemyInLastLevel = new EnemyInLastLevel()
            //{
            //    Name = Enemy.space_ship,
            //    Amount = 10,
            //    RunInfo = new RunInfo { Idx = 1 }
            //};
            //EnemyInLastLevelDB.Insert(EnemyInLastLevel);
            //Console.WriteLine($"{EnemyInLastLevelDB.SaveChanges()} affected rows.");
            //et = EnemyInLastLevelDB.SelectAll();
            //Console.WriteLine(et.Last());
            //Console.WriteLine();

            //EnemyInLastLevel = et.Last();
            //EnemyInLastLevel.Amount -= 5;
            //EnemyInLastLevelDB.Update(EnemyInLastLevel);
            //Console.WriteLine($"{EnemyInLastLevelDB.SaveChanges()} affected rows.");
            //et = EnemyInLastLevelDB.SelectAll();
            //Console.WriteLine(et.Last());
            //Console.WriteLine();

            //EnemyInLastLevelDB.Delete(et.Last());
            //Console.WriteLine($"{EnemyInLastLevelDB.SaveChanges()} affected rows.");
            //et = EnemyInLastLevelDB.SelectAll();
            //Console.WriteLine(et.Last());
            //Console.WriteLine();
            #endregion

            #region PlayersAndGroups
            //PlayersAndGroupsDB PlayerAndGroupDB = new PlayersAndGroupsDB();
            //PlayersAndGroupsTable PlayerAndGTbl = PlayerAndGroupDB.SelectAll();
            //Console.WriteLine("PlayersAndGroups: ");
            //foreach (var item in PlayerAndGTbl)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();

            //PlayerAndGroup PlayerAndGroup = new PlayerAndGroup()
            //{
            //    Player = new Player { Idx = 20 },
            //    Group = new Group { Idx = 5 }
            //};
            //PlayerAndGroupDB.Insert(PlayerAndGroup);
            //Console.WriteLine($"{PlayerAndGroupDB.SaveChanges()} affected rows.");
            //PlayerAndGTbl = PlayerAndGroupDB.SelectAll();
            //Console.WriteLine(PlayerAndGTbl.Last());
            //Console.WriteLine();

            //PlayerAndGroup = PlayerAndGTbl.Last();
            //PlayerAndGroup.Player = PlayerAndGTbl[PlayerAndGTbl.Capacity - 2].Player;
            //PlayerAndGroupDB.Update(PlayerAndGroup);
            //Console.WriteLine($"{PlayerAndGroupDB.SaveChanges()} affected rows.");
            //PlayerAndGTbl = PlayerAndGroupDB.SelectAll();
            //Console.WriteLine(PlayerAndGTbl.Last());
            //Console.WriteLine();

            //PlayerAndGroupDB.Delete(PlayerAndGTbl.Last());
            //Console.WriteLine($"{PlayerAndGroupDB.SaveChanges()} affected rows.");
            //PlayerAndGTbl = PlayerAndGroupDB.SelectAll();
            //Console.WriteLine(PlayerAndGTbl.Last());
            //Console.WriteLine();
            #endregion
        }

        public static async Task ServerFullMain()
        {
            ApiService api = new ApiService();

            Console.WriteLine("--- Starting API Demo Scenario ---\n");
            int linesChanged = 0;
            #region Admins:
            AdminsTable admins = await api.GetAllAdmins();

            // 2. Write initial list
            foreach (var item in admins)
            {
                Console.WriteLine(item + "\n");
            }

            // 3. Expected found message (Idx 2 exists)
            Console.WriteLine(await api.GetAdminByIdx(2) + "\n");

            // 4. Expected not found message (Idx 12 does not exist)
            // NOTE: The GetAdminsByIdx mock handles the error printing internally
            Admin notFoundAdminResult = await api.GetAdminByIdx(12);
            Console.WriteLine($"GetAdminByIdx(12) returned: " +
                $"{(notFoundAdminResult == null ? "NULL (Error)" :
                notFoundAdminResult.ToString())}\n");

            // 5. Insert new Admin
            linesChanged = await api.InsertAdmin(new Admin { Birthday = new DateTime(2022, 3, 2) });
            Console.WriteLine($"InsertAdmins Result (Rows Affected): {linesChanged}\n");

            // 6. Get All (Updated list)
            admins = await api.GetAllAdmins();

            // 7. Write last item (the newly inserted Admin)
            Console.WriteLine(admins.Last() + "\n");

            //8.Update the new Admin:
            //find the admin(that we just added)
            linesChanged = await api.UpdateAdmin(
                        //create DTO from entity with nulls and change only what we want
                        AdminDTO.FromEntity(admins.Last(), dto =>
                        {
                            // Define ALL changes rest is null
                            dto.Id = "14214431";
                        }
                    )
                );
            Console.WriteLine($"UpdateAdmins Result (Rows Affected): {linesChanged}\n");

            // 9. Get All (Updated list)
            admins = await api.GetAllAdmins();

            // 10. Write last item (the updated Admin)
            Console.WriteLine(admins.Last() + "\n");

            // 11. Delete the new Admin
            linesChanged = await api.DeleteAdmin(admins.Last().Idx);
            Console.WriteLine($"DeleteAdmin Result (Rows Affected):" +
                $" {linesChanged}\n");
            // 9. Get All (Updated list)
            admins = await api.GetAllAdmins();

            // 10. Write last item (the updated Admin)
            Console.WriteLine(admins.Last() + "\n");
            #endregion

            #region EnemiesInLastLevel:
            //1.Get All(Initial list)
            EnemiesInLastLevelTable enemiesInLastLevel = await api.GetAllEnemiesInLastLevel();

            //2.Write initial list
            foreach (var item in enemiesInLastLevel)
            {
                Console.WriteLine(item + "\n");
            }

            //3.Expected found message(Idx 2 exists)
            Console.WriteLine(await api.GetEnemyInLastLevelByIdx(2) + "\n");

            //4.Expected not found message(Idx 12 does not exist)
            EnemyInLastLevel notFoundEnemyInLastLevelResult = await api.GetEnemyInLastLevelByIdx(12);
            Console.WriteLine($"GetEnemyInLastLevelByIdx(12) returned: " +
                $"{(notFoundEnemyInLastLevelResult == null ? "NULL (Error)" :
                notFoundEnemyInLastLevelResult.ToString())}\n");

            linesChanged = 0;
            //5.Insert new EnemyInLastLevel
            linesChanged = await api.InsertEnemyInLastLevel(
                new EnemyInLastLevel
                {
                    Amount = 10,
                    RunInfo = new RunInfo
                    {
                        Idx = 1,
                        Player = new Player
                        {
                            Idx = 11
                        }
                    }
                }
            );
            Console.WriteLine($"InsertEnemiesInLastLevel Result (Rows Affected): {linesChanged}\n");

            //6.Get All(Updated list)
            enemiesInLastLevel = await api.GetAllEnemiesInLastLevel();

            //7.Write last item(the newly inserted EnemyInLastLevel)
            Console.WriteLine(enemiesInLastLevel.Last() + "\n");

            //8.Update the new EnemyInLastLevel:
            //find the enemyInLastLevel(that we just added)
            linesChanged = await api.UpdateEnemyInLastLevel(
                    //create DTO from entity with nulls and change only what we want
                    EnemyInLastLevelDTO.FromEntity(
                        enemiesInLastLevel.Last(), dto =>
                        {
                            //Define ALL changes, the rest is null
                            dto.Amount = 3;
                            dto.RunInfo = new RunInfoDTO
                            {
                                Idx = 2,
                            };
                        }
                    )
                );
            Console.WriteLine($"UpdateEnemiesInLastLevel Result (Rows Affected): {linesChanged}\n");

            //9.Get All(Updated list)
            enemiesInLastLevel = await api.GetAllEnemiesInLastLevel();

            //10.Write last item(the updated EnemyInLastLevel)
            Console.WriteLine(enemiesInLastLevel.Last() + "\n");

            //11.Delete the new EnemyInLastLevel
            linesChanged = await api.DeleteEnemyInLastLevel(enemiesInLastLevel.Last().Idx);
            Console.WriteLine($"DeleteEnemyInLastLevel Result " +
                $"(Rows Affected): {linesChanged}\n");
            #endregion

            #region Groups:
            // 1. Get All Groups
            GroupsTable groups = await api.GetAllGroups();

            // 2. Display initial list
            foreach (var item in groups)
            {
                Console.WriteLine(item + "\n");
            }

            // 3. Insert new Group
            linesChanged = await api.InsertGroup(new Group { Name = "Alpha Team", Score = 100 });
            Console.WriteLine($"InsertGroup Result (Rows Affected): {linesChanged}\n");

            // 4. Update the Group partially
            // We want to change the Name but keep the Score as it is in the DB.
            groups = await api.GetAllGroups();
            Console.WriteLine($"Inserted Group: {groups.Last()}\n");

            linesChanged = await api.UpdateGroup(
                GroupDTO.FromEntity(groups.Last(), dto =>
                {
                    // Only changing the Name. Score remains null in the DTO, 
                    // meaning it won't be overwritten in the DB.
                    dto.Name = "Omega Team";
                })
            );
            Console.WriteLine($"UpdateGroup Result (Rows Affected): {linesChanged}\n");

            // 5. Verify the update
            groups = await api.GetAllGroups();
            Console.WriteLine($"Updated Group: {groups.Last()}\n");

            // 6. Delete the Group
            linesChanged = await api.DeleteGroup(groups.Last().Idx);
            Console.WriteLine($"DeleteGroup Result (Rows Affected): {linesChanged}\n");
            #endregion

            #region Player:
            // 1. Get All Players
            PlayersTable players = await api.GetAllPlayers();

            // 2. Display initial list
            foreach (var item in players)
            {
                Console.WriteLine(item + "\n");
            }

            // 3. Insert new Player
            // Inherits from User properties + Player specific ones
            linesChanged = await api.InsertPlayer(new Player
            {
                Username = "pro_player_X",
                Password = "player_hash_888",
                Email = "pro@game.com",
                Id = "123123123",
                TotalScore = 0,
                MaxLevel = 1,
                IsSoundOn = true,
            });
            Console.WriteLine($"InsertPlayer Result (Rows Affected): {linesChanged}\n");

            players = await api.GetAllPlayers();
            Console.WriteLine($"Inserted Player: {players.Last()}\n");

            // 4. Perform a Partial Update (Change Score and Settings)
            // Using PlayerDTO.FromEntity to ensure only changed fields are sent
            linesChanged = await api.UpdatePlayer(
                PlayerDTO.FromEntity(players.Last(), dto =>
                {
                    dto.TotalScore = 99;
                    dto.MaxLevel = 10;
                    dto.IsSoundOn = false; // Player turned off sound
                })
            );
            Console.WriteLine($"UpdatePlayer Result (Rows Affected): {linesChanged}\n");

            // 5. Verify the update
            players = await api.GetAllPlayers();
            Console.WriteLine($"Updated Player: {players.Last()}\n");

            // 6. Delete the Player
            linesChanged = await api.DeletePlayer(players.Last().Idx);
            Console.WriteLine($"DeletePlayer Result (Rows Affected): {linesChanged}\n");

            // 7. Verify the delete
            players = await api.GetAllPlayers();
            Console.WriteLine($"Delete Player: {players.Last()}\n");
            #endregion

            #region PlayersAndGroups:
            // 1. Get initial data
            PlayersAndGroupsTable playersAndGroups = await api.GetAllPlayersAndGroups();

            // 2. Display initial list
            foreach (var item in playersAndGroups)
            {
                Console.WriteLine(item + "\n");
            }

            // 3. Insert new PlayerAndGroup
            linesChanged = await api.InsertPlayerAndGroup(
                    new PlayerAndGroup
                    {
                        Player = new Player { Idx = 11 },
                        Group = new Group { Idx = 4 }
                    }
                );
            Console.WriteLine($"Insert Player And Group Result" +
                $" (Rows Affected): {linesChanged}\n");
            playersAndGroups = await api.GetAllPlayersAndGroups();
            Console.WriteLine($"Inserted Player And Group: " +
                $"{playersAndGroups.Last()}");

            // 3. Perform a Update
            linesChanged = await api.UpdatePlayerAndGroup(
                PlayerAndGroupDTO.FromEntity(
                    playersAndGroups.Last(), dto =>
                    {
                        dto.Group = new GroupDTO { Idx = 3 };
                    }
                )
            );

            Console.WriteLine($"Update Result: {linesChanged}" +
                $" row(s) affected.");
            playersAndGroups = await api.GetAllPlayersAndGroups();
            Console.WriteLine($"Updated Player And Group:" +
                $" {playersAndGroups.Last()}\n");

            // 6. Delete the Group
            linesChanged = await api.DeletePlayerAndGroup(
            playersAndGroups.Last().Idx);
            Console.WriteLine($"Delete Player And Group Result " +
                $"(Rows Affected): {linesChanged}\n");
            playersAndGroups = await api.GetAllPlayersAndGroups();
            Console.WriteLine($"Last Player And Group:" +
                $" {playersAndGroups.Last()}\n");
            #endregion

            #region ProfileEditRequests:
            // 1. Get initial data
            ProfileEditRequestsTable profileEditRequests =
                await api.GetAllProfileEditRequests();

            // 2. Display initial list
            foreach (var item in profileEditRequests)
            {
                Console.WriteLine(item + "\n");
            }

            // 3. Insert new Group
            linesChanged = await api.InsertProfileEditRequest(
                    new ProfileEditRequest
                    {
                        RequestingDate = DateTime.Now,
                    }
                );
            Console.WriteLine($"InsertGroup Result" +
                $" (Rows Affected): {linesChanged}\n");
            profileEditRequests =
                await api.GetAllProfileEditRequests();
            Console.WriteLine($"Inserted Player And Group: " +
                $"{profileEditRequests.Last()}");

            // 3. Perform a Update
            linesChanged = await api.UpdateProfileEditRequest(
                ProfileEditRequestDTO.FromEntity(
                    profileEditRequests.Last(), dto =>
                    {
                        dto.RequestingPlayer =
                            new PlayerDTO()
                            {
                                Idx = 14,
                            };
                    }
                )
            );

            Console.WriteLine($"Update Result: {linesChanged}" +
                $" row(s) affected.");
            profileEditRequests = await api.GetAllProfileEditRequests();
            Console.WriteLine($"Updated Player And Group:" +
                $" {profileEditRequests.Last()}");
            // 6. Delete the Group
            linesChanged = await api.DeleteProfileEditRequest(
            profileEditRequests.Last().Idx);
            Console.WriteLine($"Delete Player And Group Result " +
                $"(Rows Affected): {linesChanged}\n");
            #endregion

            #region RequestData:
            // 1. Get All RequestData
            RequestsDataTable requestDataItems =
                await api.GetAllRequestsData();

            // 2. Display initial list
            foreach (var item in requestDataItems)
            {
                Console.WriteLine(item + "\n");
            }

            //3.Insert new RequestData
            linesChanged = await api.InsertRequestData(new RequestData
            {
                Request = new ProfileEditRequest
                {
                    Idx = 6,
                    RequestingPlayer = new Player { Idx = 20 },
                    AdressingAdmin = new Admin { Idx = 8 },
                },
                Field = "Password",
                OldValue = "hashed_pass_P20",
                NewValue = "another_new_hash"
            });
            Console.WriteLine($"InsertRequestData Result (Rows Affected): {linesChanged}\n");

            requestDataItems = await api.GetAllRequestsData();
            Console.WriteLine($"Inserted RequestData: {requestDataItems.Last()}\n");

            // 4. Perform a Partial Update (Reference Switch + Value Change)
            linesChanged = await api.UpdateRequestData(
                RequestDataDTO.FromEntity(requestDataItems.Last(), dto =>
                {
                    // Switch the connected request to 3 and change the value
                    dto.Request = new ProfileEditRequestDTO { Idx = 3 };
                    dto.NewValue = "99";
                })
            );
            Console.WriteLine($"UpdateRequestData Result (Rows Affected): {linesChanged}\n");

            // 5. Verify the update
            requestDataItems = await api.GetAllRequestsData();
            Console.WriteLine($"Updated RequestData: {requestDataItems.Last()}\n");

            // 6. Delete the RequestData
            linesChanged = await api.DeleteRequestData(requestDataItems.Last().Idx);
            Console.WriteLine($"DeleteRequestData Result (Rows Affected): {linesChanged}\n");
            // 5. Verify the delete
            requestDataItems = await api.GetAllRequestsData();
            Console.WriteLine($"Last RequestData: {requestDataItems.Last()}\n");
            #endregion

            #region RunsInfo:
            // 1. Get All RunInfo
            RunsInfoTable runs = await api.GetAllRunsInfo();

            // 2. Display initial list
            foreach (var item in runs)
            {
                Console.WriteLine(item + "\n");
            }

            // 3. Insert new RunInfo
            linesChanged = await api.InsertRunInfo(new RunInfo
            {
                Player = new Player { Idx = 16 },
                CurrentScore = 0,
                CurrentLevel = 1,
                CurrentHp = 5,
                IsRunOver = false
            });
            Console.WriteLine($"InsertRunInfo Result (Rows Affected): {linesChanged}\n");

            runs = await api.GetAllRunsInfo();
            Console.WriteLine($"Inserted Run: {runs.Last()}\n");

            // 4. Perform a Partial Update (Reference Switch + Score Update)
            // Switching the run to a different Player (Idx 3) and updating Score/Level
            linesChanged = await api.UpdateRunInfo(
                RunInfoDTO.FromEntity(runs.Last(), dto =>
                {
                    // Pivot: Switch the Player associated with this run
                    dto.Player = new PlayerDTO { Idx = 13 };

                    // Update stats
                    dto.CurrentScore = 1500;
                    dto.CurrentLevel = 4;
                    dto.CurrentHp = 3;
                })
            );
            Console.WriteLine($"UpdateRunInfo Result (Rows Affected): {linesChanged}\n");

            // 5. Verify the update
            runs = await api.GetAllRunsInfo();
            Console.WriteLine($"Updated Run: {runs.Last()}\n");

            // 6. Delete the RunInfo
            linesChanged = await api.DeleteRunInfo(runs.Last().Idx);
            Console.WriteLine($"DeleteRunInfo Result (Rows Affected): {linesChanged}\n");
            runs = await api.GetAllRunsInfo();
            Console.WriteLine($"Last Run: {runs.Last()}\n");
            #endregion

            #region Users:
            // 1. Get All Users
            UsersTable users = await api.GetAllUsers();

            // 2. Display initial list
            foreach (var item in users)
            {
                Console.WriteLine(item + "\n");
            }

            // 3. Insert new User
            linesChanged = await api.InsertUser(new User
            {
                Username = "new_test_user",
                Password = "hashed_password_123",
                Email = "test@user.com",
                Id = "312345678",
                Birthday = new DateTime(1995, 10, 10),
                IsLoggedIn = false
            });
            Console.WriteLine($"InsertUser Result (Rows Affected): {linesChanged}\n");

            users = await api.GetAllUsers();
            Console.WriteLine($"Inserted User: {users.Last()}\n");

            // 4. Perform a Partial Update (Change Username and Email)
            // Password, Id, and Birthday remain null in the DTO action,
            // so they are not sent to the server and remain unchanged in DB.
            linesChanged = await api.UpdateUser(
                UserDTO.FromEntity(users.Last(), dto =>
                {
                    dto.Username = "updated_username_99";
                    dto.Email = "updated_email@user.com";
                    dto.IsLoggedIn = true;
                })
            );
            Console.WriteLine($"UpdateUser Result (Rows Affected): {linesChanged}\n");

            // 5. Verify the update
            users = await api.GetAllUsers();
            Console.WriteLine($"Updated User: {users.Last()}\n");

            // 6. Delete the User
            linesChanged = await api.DeleteUser(users.Last().Idx);
            Console.WriteLine($"DeleteUser Result (Rows Affected): {linesChanged}\n");
            // 5. Verify the update
            users = await api.GetAllUsers();
            Console.WriteLine($"Last User: {users.Last()}\n");
            #endregion

            Console.WriteLine("--- API Demo Scenario Complete ---");
        }
    }
}
