﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Entitys;
using Model.Tables;
using ViewModel;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region users
            UsersDB UserDB = new UsersDB();
            UsersTable ut = UserDB.SelectAll();
            Console.WriteLine("users: ");
            foreach (var t in ut)
            {
                Console.WriteLine(t);
            }
            Console.WriteLine();

            //User user = new User() { Id = "122431414" };
            //UserDB.Insert(user);
            //Console.WriteLine($"{UserDB.SaveChanges()} affected rows.");
            //ut = UserDB.SelectAll();
            //foreach (var t in ut)
            //{
            //    Console.WriteLine(t);
            //}

            //user = ut.Last();
            //user.Username = "user245";
            //UserDB.Update(user);
            //Console.WriteLine($"{UserDB.SaveChanges()} affected rows.");
            //ut = UserDB.SelectAll();
            //foreach (var t in ut)
            //{
            //    Console.WriteLine(t);
            //}

            //UserDB.Delete(ut.Last());
            //Console.WriteLine($"{UserDB.SaveChanges()} affected rows.");
            //ut = UserDB.SelectAll();
            //foreach (var t in ut)
            //{
            //    Console.WriteLine(t);
            //}
            #endregion

            #region players
            Console.WriteLine("players: ");
            PlayersDB PlayerDB = new PlayersDB();
            PlayersTable pt = PlayerDB.SelectAll();
            foreach (var item in pt)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            //Player Player = new Player() { Id = "454252552" };
            //PlayerDB.Insert(Player);
            //Console.WriteLine($"{PlayerDB.SaveChanges()} affected rows.");
            //pt = PlayerDB.SelectAll();
            //Console.WriteLine(pt.Last());

            //Player = pt.Last();
            //Player.Username = "Player245";
            //PlayerDB.Update(Player);
            //Console.WriteLine($"{PlayerDB.SaveChanges()} affected rows.");
            //pt = PlayerDB.SelectAll();
            //Console.WriteLine(pt.Last());

            //PlayerDB.Delete(pt.Last());
            //Console.WriteLine($"{PlayerDB.SaveChanges()} affected rows.");
            //pt = PlayerDB.SelectAll();
            //foreach (var t in pt)
            //{
            //    Console.WriteLine(t);
            //}
            #endregion

            #region admins
            AdminsDB AdminDB = new AdminsDB();
            AdminsTable at = AdminDB.SelectAll();
            Console.WriteLine("admins: ");
            foreach (var item in at)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            //Admin Admin = new Admin() { Id = "454252552" };
            //AdminDB.Insert(Admin);
            //Console.WriteLine($"{AdminDB.SaveChanges()} affected rows.");
            //at = AdminDB.SelectAll();
            //Console.WriteLine(at.Last());

            //Admin = at.Last();
            //Admin.Username = "Admin245";
            //AdminDB.Update(Admin);
            //Console.WriteLine($"{AdminDB.SaveChanges()} affected rows.");
            //at = AdminDB.SelectAll();
            //Console.WriteLine(at.Last());

            //AdminDB.Delete(at.Last());
            //Console.WriteLine($"{AdminDB.SaveChanges()} affected rows.");
            //at = AdminDB.SelectAll();
            //foreach (var t in at)
            //{
            //    Console.WriteLine(t);
            //}
            #endregion

            #region RequestData
            RequestsDataDB RequestsDataDB = new RequestsDataDB();
            RequestsDataTable rt = RequestsDataDB.SelectAll();
            Console.WriteLine("RequestsData: ");
            foreach (var item in rt)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            //RequestData RequestsData = new RequestData()
            //{
            //    Request = new ProfileEditRequest { Idx = 1 },
            //    Field = "3654"
            //};
            //RequestsDataDB.Insert(RequestsData);
            //Console.WriteLine($"{RequestsDataDB.SaveChanges()} affected rows.");
            //rt = RequestsDataDB.SelectAll();
            //Console.WriteLine(rt.Last());

            //RequestsData = rt.Last();
            //RequestsData.NewValue = "111";
            //RequestsDataDB.Update(RequestsData);
            //Console.WriteLine($"{RequestsDataDB.SaveChanges()} affected rows.");
            //rt = RequestsDataDB.SelectAll();
            //Console.WriteLine(rt.Last());

            //RequestsDataDB.Delete(rt.Last());
            //Console.WriteLine($"{RequestsDataDB.SaveChanges()} affected rows.");
            //rt = RequestsDataDB.SelectAll();
            //foreach (var t in rt)
            //{
            //    Console.WriteLine(t);
            //}
            #endregion

            #region ProfileEditRequests
            ProfileEditRequestsDB ProfileEditRequestDB = new ProfileEditRequestsDB();
            ProfileEditRequestsTable ProfTbl = ProfileEditRequestDB.SelectAll();
            Console.WriteLine("ProfileEditRequests: ");
            foreach (var item in ProfTbl)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            //ProfileEditRequest ProfileEditRequest = new ProfileEditRequest() { RequestingPlayer = new Player() { Idx = 3 }, Status = Status.Pending };
            //ProfileEditRequestDB.Insert(ProfileEditRequest);
            //Console.WriteLine($"{ProfileEditRequestDB.SaveChanges()} affected rows.");
            //ProfTbl = ProfileEditRequestDB.SelectAll();
            //Console.WriteLine(ProfTbl.Last());

            //ProfileEditRequest = ProfTbl.Last();
            //ProfileEditRequest.Status = Status.Approved;
            //ProfileEditRequestDB.Update(ProfileEditRequest);
            //Console.WriteLine($"{ProfileEditRequestDB.SaveChanges()} affected rows.");
            //ProfTbl = ProfileEditRequestDB.SelectAll();
            //Console.WriteLine(ProfTbl.Last());

            //ProfileEditRequestDB.Delete(ProfTbl.Last());
            //Console.WriteLine($"{ProfileEditRequestDB.SaveChanges()} affected rows.");
            //ProfTbl = ProfileEditRequestDB.SelectAll();
            //Console.WriteLine(ProfTbl.Last());
            #endregion

            #region Groups
            GroupsDB GroupDB = new GroupsDB();
            GroupsTable gt = GroupDB.SelectAll();
            Console.WriteLine("Groups: ");
            foreach (var item in gt)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            //Group group = new Group() { GroupScore = 1 };
            //GroupDB.Insert(group);
            //Console.WriteLine($"{GroupDB.SaveChanges()} affected rows.");
            //gt = GroupDB.SelectAll();
            //Console.WriteLine(gt.Last());

            //group = gt.Last();
            //group.GroupScore += 100;
            //GroupDB.Update(group);
            //Console.WriteLine($"{GroupDB.SaveChanges()} affected rows.");
            //gt = GroupDB.SelectAll();
            //Console.WriteLine(gt.Last());

            //GroupDB.Delete(gt.Last());
            //Console.WriteLine($"{GroupDB.SaveChanges()} affected rows.");
            //gt = GroupDB.SelectAll();
            //Console.WriteLine(gt.Last());
            #endregion

            #region RunsInfo
            RunsInfoDB RunInfoDB = new RunsInfoDB();
            RunsInfoTable RunTbl = RunInfoDB.SelectAll();
            Console.WriteLine("RunsInfo: ");
            foreach (var item in RunTbl)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            //RunInfo RunInfo = new RunInfo() { Player =
            //new Player {Idx = 3 }, CurrentLevel=1 };
            //RunInfoDB.Insert(RunInfo);
            //Console.WriteLine($"{RunInfoDB.SaveChanges()} affected rows.");
            //RunTbl = RunInfoDB.SelectAll();
            //Console.WriteLine(RunTbl.Last());

            //RunInfo = RunTbl.Last();
            //RunInfo.CurrentLevel += 5;
            //RunInfoDB.Update(RunInfo);
            //Console.WriteLine($"{RunInfoDB.SaveChanges()} affected rows.");
            //RunTbl = RunInfoDB.SelectAll();
            //Console.WriteLine(RunTbl.Last());

            //RunInfoDB.Delete(RunTbl.Last());
            //Console.WriteLine($"{RunInfoDB.SaveChanges()} affected rows.");
            //RunTbl = RunInfoDB.SelectAll();
            //Console.WriteLine(RunTbl.Last());
            #endregion

            #region EnemiesInLastLevel
            EnemiesInLastLevelDB EnemyInLastLevelDB = new EnemiesInLastLevelDB();
            EnemiesInLastLevelTable et = EnemyInLastLevelDB.SelectAll();
            Console.WriteLine("EnemiesInLastLevel: ");
            foreach (var item in et)
            {
                Console.WriteLine(item );
            }
            Console.WriteLine();

            //EnemyInLastLevel EnemyInLastLevel = new EnemyInLastLevel() {
            //    Name = Enemy.space_ship,
            //    Amount = 10,
            //    RunInfo = new RunInfo { Idx = 1 }
            //};
            //EnemyInLastLevelDB.Insert(EnemyInLastLevel);
            //Console.WriteLine($"{EnemyInLastLevelDB.SaveChanges()} affected rows.");
            //et = EnemyInLastLevelDB.SelectAll();
            //Console.WriteLine(et.Last());

            //EnemyInLastLevel = et.Last();
            //EnemyInLastLevel.Amount -= 5;
            //EnemyInLastLevelDB.Update(EnemyInLastLevel);
            //Console.WriteLine($"{EnemyInLastLevelDB.SaveChanges()} affected rows.");
            //et = EnemyInLastLevelDB.SelectAll();
            //Console.WriteLine(et.Last());

            //EnemyInLastLevelDB.Delete(et.Last());
            //Console.WriteLine($"{EnemyInLastLevelDB.SaveChanges()} affected rows.");
            //et = EnemyInLastLevelDB.SelectAll();
            //Console.WriteLine(et.Last());
            #endregion

            #region PlayersAndGroups
            PlayersAndGroupsDB PlayerAndGroupDB = new PlayersAndGroupsDB();
            PlayersAndGroupsTable PlayerAndGTbl = PlayerAndGroupDB.SelectAll();
            Console.WriteLine("PlayersAndGroups: ");
            foreach (var item in PlayerAndGTbl)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            //PlayerAndGroup PlayerAndGroup = new PlayerAndGroup()
            //{
            //    Player = new Player { Idx = 19 },
            //    Group = new Group { Idx = 3 }
            //};
            //PlayerAndGroupDB.Insert(PlayerAndGroup);
            //Console.WriteLine($"{PlayerAndGroupDB.SaveChanges()} affected rows.");
            //PlayerAndGTbl = PlayerAndGroupDB.SelectAll();
            //Console.WriteLine(PlayerAndGTbl.Last());

            //PlayerAndGroup = PlayerAndGTbl.Last();
            //PlayerAndGroup.Player = PlayerAndGTbl[PlayerAndGTbl.Capacity-2].Player;
            //PlayerAndGroupDB.Update(PlayerAndGroup);
            //Console.WriteLine($"{PlayerAndGroupDB.SaveChanges()} affected rows.");
            //PlayerAndGTbl = PlayerAndGroupDB.SelectAll();
            //Console.WriteLine(PlayerAndGTbl.Last());

            //PlayerAndGroupDB.Delete(PlayerAndGTbl.Last());
            //Console.WriteLine($"{PlayerAndGroupDB.SaveChanges()} affected rows.");
            //PlayerAndGTbl = PlayerAndGroupDB.SelectAll();
            //Console.WriteLine(PlayerAndGTbl.Last());
            #endregion
        }
    }
}
