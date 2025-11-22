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
            p.Score = int.Parse(reader["Score"].ToString());
            p.Name = reader["Name"].ToString();
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new Group();
        }

        static private GroupsTable list = new GroupsTable();
        public static Group SelectByIdx(int idx)
        {
            GroupsDB db = new GroupsDB();
            list = db.SelectAll();

            Group g = list.Find(item => (item.Idx == idx));
            if (g == null)
            {
                throw new Exception($"Group with Idx {idx} not found.");
            }
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
                string sqlStr = $"INSERT INTO dbo.GroupsTbl(Score,Name) " +
                        $"VALUES (@Score,@Name)";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@Score", c.Score));
                command.Parameters.Add(new SqlParameter("@Name", c.Name));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand command)
        {
            Group c = entity as Group;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.GroupsTbl SET Score=@Score, Name=@Name WHERE Idx=@Idx";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@Score", c.Score));
                command.Parameters.Add(new SqlParameter("@Name", c.Name));
                command.Parameters.Add(new SqlParameter("@Idx", c.Idx));
            }
        }
    }
}