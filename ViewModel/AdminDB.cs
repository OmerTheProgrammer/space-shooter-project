using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using System.Data.Sql;
using Model.Tables;
using Model.Entitys;
using Microsoft.Data.SqlClient;

namespace ViewModel
{
    public class AdminsDB : UsersDB
    {
        public AdminsTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM (AdminsTbl INNER JOIN\r\n UsersTbl ON AdminsTbl.Idx = UsersTbl.Idx)";
            AdminsTable pList = new AdminsTable(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Admin p = entity as Admin;
            p.StartDate = DateTime.Parse(reader["StartDate"].ToString());
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new Admin();
        }

        static private AdminsTable list = new AdminsTable();
        public static Admin SelectById(int id)
        {
            AdminsDB db = new AdminsDB();
            list = db.SelectAll();

            Admin g = list.Find(item => (item.Idx == id));
            return g;
        }

        //שלב ב
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

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand cmd)
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

        public override void Insert(BaseEntity entity)
        {
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null & entity.GetType() == reqEntity.GetType())
            {
                inserted.Add(new ChangeEntity(base.CreateInsertdSQL, entity));
                inserted.Add(new ChangeEntity(this.CreateInsertdSQL, entity));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd)
        {
            Admin c = entity as Admin;
            if (c != null)
            {
                string sqlStr = $"UPDATE AdminsTbl SET StartDate=@StartDate, WHERE Idx=@idx";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@idx", c.Idx));
                command.Parameters.Add(new SqlParameter("@StartDate", c.StartDate));
            }
        }

        public override void Update(BaseEntity entity)
        {
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                updated.Add(new ChangeEntity(base.CreateUpdatedSQL, entity));
                updated.Add(new ChangeEntity(this.CreateUpdatedSQL, entity));
            }
        }
    }
}