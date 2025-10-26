using System;
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
            //UsersDB UserDB = new UsersDB();
            //UsersTable ut = UserDB.SelectAll();
            //foreach (var t in ut)
            //{
            //    Console.WriteLine(t);
            //}
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
            //PlayersDB PlayerDB = new PlayersDB();
            //PlayersTable pt = PlayerDB.SelectAll();

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
            //AdminsDB AdminDB = new AdminsDB();
            //AdminsTable pt = AdminDB.SelectAll();

            //Admin Admin = new Admin() { Id = "454252552" };
            //AdminDB.Insert(Admin);
            //Console.WriteLine($"{AdminDB.SaveChanges()} affected rows.");
            //pt = AdminDB.SelectAll();
            //Console.WriteLine(pt.Last());

            //Admin = pt.Last();
            //Admin.Username = "Admin245";
            //AdminDB.Update(Admin);
            //Console.WriteLine($"{AdminDB.SaveChanges()} affected rows.");
            //pt = AdminDB.SelectAll();
            //Console.WriteLine(pt.Last());

            //AdminDB.Delete(pt.Last());
            //Console.WriteLine($"{AdminDB.SaveChanges()} affected rows.");
            //pt = AdminDB.SelectAll();
            //foreach (var t in pt)
            //{
            //    Console.WriteLine(t);
            //}
            #endregion

            #region RequestData
            //RequestsDataDB RequestsDataDB = new RequestsDataDB();
            //RequestsDataTable pt = RequestsDataDB.SelectAll();
            //foreach (var item in pt)
            //{
            //    Console.WriteLine(item);
            //}

            //RequestData RequestsData = new RequestData()
            //{
            //    Request = new ProfileEditRequest { Idx = 1 },
            //    Field = "3654"
            //};
            //RequestsDataDB.Insert(RequestsData);
            //Console.WriteLine($"{RequestsDataDB.SaveChanges()} affected rows.");
            //pt = RequestsDataDB.SelectAll();
            //Console.WriteLine(pt.Last());

            //RequestsData = pt.Last();
            //RequestsData.NewValue = "111";
            //RequestsDataDB.Update(RequestsData);
            //Console.WriteLine($"{RequestsDataDB.SaveChanges()} affected rows.");
            //pt = RequestsDataDB.SelectAll();
            //Console.WriteLine(pt.Last());

            //RequestsDataDB.Delete(pt.Last());
            //Console.WriteLine($"{RequestsDataDB.SaveChanges()} affected rows.");
            //pt = RequestsDataDB.SelectAll();
            //foreach (var t in pt)
            //{
            //    Console.WriteLine(t);
            //}
            #endregion

            #region ProfileEditRequests
            ProfileEditRequestsDB ProfileEditRequestDB = new ProfileEditRequestsDB();
            ProfileEditRequestsTable pt = ProfileEditRequestDB.SelectAll();
            foreach (var item in pt)
            {
                Console.WriteLine(item);
            }

            ProfileEditRequest ProfileEditRequest = new ProfileEditRequest() { RequestingPlayer = new Player() { Idx = 3 }, Status = Status.Pending };
            //ProfileEditRequestDB.Insert(ProfileEditRequest);
            //Console.WriteLine($"{ProfileEditRequestDB.SaveChanges()} affected rows.");
            //pt = ProfileEditRequestDB.SelectAll();
            //Console.WriteLine(pt.Last());

            //ProfileEditRequest = pt.Last();
            //ProfileEditRequest.Status = Status.Approved;
            //ProfileEditRequestDB.Update(ProfileEditRequest);
            //Console.WriteLine($"{ProfileEditRequestDB.SaveChanges()} affected rows.");
            //pt = ProfileEditRequestDB.SelectAll();
            //Console.WriteLine(pt.Last());

            //ProfileEditRequestDB.Delete(pt.Last());
            //Console.WriteLine($"{ProfileEditRequestDB.SaveChanges()} affected rows.");
            //pt = ProfileEditRequestDB.SelectAll();
            //foreach (var t in pt)
            //{
            //    Console.WriteLine(t);
            //}
            #endregion

        }
    }
}
