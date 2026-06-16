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

namespace ViewModel.DBs
{
    public class AdminsDB : UsersDB
    {
        public AdminsTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM (AdminsTbl INNER JOIN\r\n" +
                $" UsersTbl ON AdminsTbl.Idx = UsersTbl.Idx) ORDER BY UsersTbl.Idx ASC";
            AdminsTable pList = new AdminsTable(Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Admin p = entity as Admin;
            p.StartDate = DateTime.Parse(reader["StartDate"].ToString());
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new Admin();
        }
        public static Admin SelectByIdx(int idx)
        {
            AdminsDB db = new AdminsDB();
            AdminsTable list = db.SelectAll();

            Admin g = list.Find(item => item.Idx == idx);
            if (g == null)
            {
                throw new ExpandedException($"Admin with Idx {idx} not found.");
            }
            return g;
        }

        //שלב ב
        public override void Delete(BaseEntity entity)
        {
            if (entity == null) return;

            //if both player and admin on the same user
            if (PlayersDB.SelectByIdx(entity.Idx) != null)
            {
                //delete only admin
                changes.Add(new ChangeEntity(entity, DbAction.Delete));
            }
            else
            {
                //delete admin and user
                changes.Add(new ChangeEntity(entity, DbAction.DeleteFather));
                changes.Add(new ChangeEntity(entity, DbAction.Delete));
            }
        }

        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd)
        {
            Admin c = entity as Admin;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM AdminsTbl where idx=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@pid", c.Idx));
            }
        }

        protected override void CreateDeletedFatherSQL(BaseEntity entity, SqlCommand cmd)
        {
            base.CreateDeletedSQL(entity, cmd);
        }


        public override void Insert(BaseEntity entity)
        {
            if (entity == null) return;

            // אם המשתמש כבר קיים בטבלת השחקנים - הוא כבר קיים בטבלת האב (Users)!
            if (PlayersDB.SelectByIdx(entity.Idx) != null)
            {
                // לכן מכניסים רק לטבלת הילד (Admins)
                changes.Add(new ChangeEntity(entity, DbAction.InsertChild));
            }
            else
            {
                // אם הוא לא קיים בשחקנים, צריך להכניס אותו גם לאב וגם לילד
                changes.Add(new ChangeEntity(entity, DbAction.InsertFather));
                changes.Add(new ChangeEntity(entity, DbAction.InsertChild));
            }
        }

        protected override void CreateInsertedSQL(BaseEntity entity, SqlCommand cmd)
        {
            Admin c = entity as Admin;
            if (c != null)
            {
                string sqlStr = $"Insert INTO  AdminsTbl (Idx,StartDate) VALUES " +
                    $"(@idx,@StartDate)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@idx", c.Idx));
                command.Parameters.Add(new SqlParameter("@StartDate", c.StartDate));
            }
        }

        protected override void CreateInsertedFatherSQL(BaseEntity entity, SqlCommand cmd)
        {
            base.CreateInsertedSQL(entity, cmd); // קורא ל-SQL של UsersDB
        }

        public override void Update(BaseEntity entity)
        {
            if (entity == null) return;

            // מעדכנים את שניהם בכל מקרה
            changes.Add(new ChangeEntity(entity, DbAction.UpdateFather));
            changes.Add(new ChangeEntity(entity, DbAction.Update));
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd)
        {
            Admin c = entity as Admin;
            if (c != null)
            {
                string sqlStr = $"UPDATE AdminsTbl SET StartDate=@StartDate WHERE Idx=@idx";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@idx", c.Idx));
                command.Parameters.Add(new SqlParameter("@StartDate", c.StartDate));
            }
        }

        protected override void CreateUpdatedFatherSQL(BaseEntity entity, SqlCommand cmd)
        {
            base.CreateUpdatedSQL(entity, cmd); // קורא ל-SQL של UsersDB
        }
    }
}