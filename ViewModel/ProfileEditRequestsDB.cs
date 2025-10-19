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
    public class ProfileEditRequestsDB : BaseDB
    {
        public ProfileEditRequestsTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM ProfileEditRequestsTbl";
            ProfileEditRequestsTable pList = new ProfileEditRequestsTable(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            ProfileEditRequest p = entity as ProfileEditRequest;
            p.RequestingPlayer = PlayersDB.SelectById((int)reader["PlayerIdx"]);
            p.AdressingAdmin = AdminsDB.SelectById((int)reader["AdminIdx"]);
            p.ReviewDate = DateTime.Parse(reader["ReviewDate"].ToString());
            p.RequestDate = DateTime.Parse(reader["RequestDate"].ToString());
            p.Status = (Status)((int)reader["Status"]);
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new ProfileEditRequest();
        }

        static private ProfileEditRequestsTable list = new ProfileEditRequestsTable();
        public static ProfileEditRequest SelectById(int id)
        {
            ProfileEditRequestsDB db = new ProfileEditRequestsDB();
            list = db.SelectAll();

            ProfileEditRequest g = list.Find(item => (item.Idx == id));
            return g;
        }

        //שלב ב
        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd)
        {
            ProfileEditRequest c = entity as ProfileEditRequest;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM ProfileEditRequestsTbl where Idx=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@pid", c.Idx));
            }
        }

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand cmd)
        {
            ProfileEditRequest c = entity as ProfileEditRequest;
            if (c != null)
            {
                /*
                string sqlStr = $"INSERT INTO dbo.ProfileEditRequestsTbl(ID, Password, ProfileEditRequestname, Birthday, Email, IsLoggedIn) " +
                        $"VALUES (@ID, @Password, @ProfileEditRequestname, @Birthday, @Email, @IsLoggedIn)";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@ID", c.Id));
                command.Parameters.Add(new SqlParameter("@Password", c.Password));
                command.Parameters.Add(new SqlParameter("@ProfileEditRequestname", c.ProfileEditRequestname));
                command.Parameters.Add(new SqlParameter("@Birthday", c.Birthday));
                command.Parameters.Add(new SqlParameter("@Email", c.Email));
                command.Parameters.Add(new SqlParameter("@IsLoggedIn", c.IsLoggedIn));
                */
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd)
        {
            ProfileEditRequest c = entity as ProfileEditRequest;
            if (c != null)
            {
                /*
                string sqlStr = $"UPDATE dbo.ProfileEditRequestsTbl SET ProfileEditRequestname=@ProfileEditRequestname, Birthday=@Birthday, IsLoggedIn=@IsLoggedIn, " +
                    $"Email=@Email, ID=@ID, Password=@Password WHERE Idx=@Idx";
                cmd.CommandText = sqlStr;

                cmd.Parameters.Add(new SqlParameter("@ProfileEditRequestname", c.ProfileEditRequestname));
                cmd.Parameters.Add(new SqlParameter("@Birthday", c.Birthday));
                cmd.Parameters.Add(new SqlParameter("@IsLoggedIn", c.IsLoggedIn));
                cmd.Parameters.Add(new SqlParameter("@Email", c.Email));
                cmd.Parameters.Add(new SqlParameter("@ID", c.Id));
                cmd.Parameters.Add(new SqlParameter("@Password", c.Password));
                cmd.Parameters.Add(new SqlParameter("@Idx", c.Idx));
                */
            }
        }
    }
}