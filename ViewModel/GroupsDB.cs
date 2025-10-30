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
    
    public class GroupsDB : BaseDB
    {
        public GroupsTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM GroupsTbl";
            GroupsTable pList = new GroupsTable(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Group p = entity as Group;
            p.GroupScore = int.Parse(reader["GroupScore"].ToString());
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new Group();
        }

        static private GroupsTable list = new GroupsTable();
        public static Group SelectById(int id)
        {
            GroupsDB db = new GroupsDB();
            list = db.SelectAll();

            Group g = list.Find(item => (item.Idx == id));
            return g;
        }

        //שלב ב
        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd)
        {
            Group c = entity as Group;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM GroupsTbl where Idx=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@pid", c.Idx));
            }
        }

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand cmd)
        {
            Group c = entity as Group;
            if (c != null)
            {
                string sqlStr = $"INSERT INTO dbo.GroupsTbl(GroupScore) " +
                        $"VALUES (@GroupScore)";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@GroupScore", c.GroupScore));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd)
        {
            Group c = entity as Group;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.GroupsTbl SET GroupScore=@GroupScore WHERE Idx=@Idx";
                cmd.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@GroupScore", c.GroupScore));
                cmd.Parameters.Add(new SqlParameter("@Idx", c.Idx));
            }
        }
    }
}