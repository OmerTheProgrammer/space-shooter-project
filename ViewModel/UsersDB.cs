using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using Model.Tables;
using Model.Entitys;
using Microsoft.Data.SqlClient;

namespace ViewModel
{
    public class UsersDB : BaseDB
    {
        public UsersTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM UsersTbl";
            UsersTable pList = new UsersTable(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            User p = entity as User;
            p.Id = reader["ID"].ToString();//protaction in insert
            p.Password = reader["Password"].ToString();
            p.Username = reader["Username"].ToString();
            p.Birthday = DateTime.Parse(reader["Birthday"].ToString());
            p.Email = reader["Email"].ToString();
            p.IsLoggedIn = bool.Parse(reader["IsLoggedIn"].ToString());
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new User();
        }

        static private UsersTable list = new UsersTable();
        public static User SelectByIdx(int idx)
        {
            UsersDB db = new UsersDB();
            list = db.SelectAll();

            User g = list.Find(item => (item.Idx == idx));
            if (g == null)
            {
                throw new Exception($"User with Idx {idx} not found.");
            }
            return g;
        }

        //שלב ב
        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd)
        {
            User c = entity as User;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM UsersTbl where Idx=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@pid", c.Idx));
            }
        }

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand cmd)
        {
            User c = entity as User;
            if (c != null)
            {
                string sqlStr = $"INSERT INTO dbo.UsersTbl(ID, Password, Username, Birthday, Email, IsLoggedIn) " +
                        $"VALUES (@ID, @Password, @Username, @Birthday, @Email, @IsLoggedIn)";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@ID", c.Id));
                command.Parameters.Add(new SqlParameter("@Password", c.Password));
                command.Parameters.Add(new SqlParameter("@Username", c.Username));
                command.Parameters.Add(new SqlParameter("@Birthday", c.Birthday));
                command.Parameters.Add(new SqlParameter("@Email", c.Email));
                command.Parameters.Add(new SqlParameter("@IsLoggedIn", c.IsLoggedIn));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd)
        {
            User c = entity as User;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.UsersTbl SET Username=@Username, Birthday=@Birthday, IsLoggedIn=@IsLoggedIn, " +
                    $"Email=@Email, ID=@ID, Password=@Password WHERE Idx=@Idx";
                cmd.CommandText = sqlStr;

                cmd.Parameters.Add(new SqlParameter("@Username", c.Username));
                cmd.Parameters.Add(new SqlParameter("@Birthday", c.Birthday));
                cmd.Parameters.Add(new SqlParameter("@IsLoggedIn", c.IsLoggedIn));
                cmd.Parameters.Add(new SqlParameter("@Email", c.Email));
                cmd.Parameters.Add(new SqlParameter("@ID", c.Id));
                cmd.Parameters.Add(new SqlParameter("@Password", c.Password));
                cmd.Parameters.Add(new SqlParameter("@Idx", c.Idx));
            }
        }
    }
}