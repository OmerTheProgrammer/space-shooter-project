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
    
    public class PlayersAndGroupsDB : BaseDB
    {
        public PlayersAndGroupsTable SelectAll()
        {
            
            command.CommandText = $"SELECT * FROM PlayersAndGroupsTbl";
            PlayersAndGroupsTable pList = new PlayersAndGroupsTable(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            PlayerAndGroup p = entity as PlayerAndGroup;
            p.Player = PlayersDB.SelectByIdx(int.Parse(reader["PlayerIdx"].ToString())); 
            p.Group = GroupsDB.SelectByIdx(int.Parse(reader["GroupIdx"].ToString()));
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new PlayerAndGroup();
        }

        static private PlayersAndGroupsTable list = new PlayersAndGroupsTable();
        public static PlayerAndGroup SelectByIdx(int idx)
        {
            PlayersAndGroupsDB db = new PlayersAndGroupsDB();
            list = db.SelectAll();

            PlayerAndGroup g = list.Find(item => (item.Idx == idx));
            if (g == null)
            {
                throw new Exception($"PlayerAndGroup with Idx {idx} not found.");
            }
            return g;
        }

        //שלב ב

        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand command)
        {
            PlayerAndGroup c = entity as PlayerAndGroup;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM PlayersAndGroupsTbl where Idx=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@pid", c.Idx));
            }
        }

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand command)
        {
            PlayerAndGroup c = entity as PlayerAndGroup;
            if (c != null)
            {
                string sqlStr = $"INSERT INTO dbo.PlayersAndGroupsTbl(PlayerIdx,GroupIdx) " +
                        $"VALUES (@PlayerIdx,@GroupIdx)";
                command.CommandText = sqlStr;

                if (c.Player != null)
                {
                    command.Parameters.Add(new SqlParameter("@PlayerIdx", c.Player.Idx));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Need Player!\n");
                    throw new Exception(message: "Need Player!");
                }
                if (c.Group != null)
                {
                    command.Parameters.Add(new SqlParameter("@GroupIdx", c.Group.Idx));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Need Group!\n");
                    throw new Exception(message: "Need Group!");
                }
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand command)
        {
            PlayerAndGroup c = entity as PlayerAndGroup;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.PlayersAndGroupsTbl SET GroupIdx=@GroupIdx," +
                    $"PlayerIdx=@PlayerIdx WHERE Idx=@Idx";
                command.CommandText = sqlStr;

                if (c.Player != null)
                {
                    command.Parameters.Add(new SqlParameter("@PlayerIdx", c.Player.Idx));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Need Player!\n");
                    throw new Exception(message: "Need Player!");
                }
                if (c.Group != null)
                {
                    command.Parameters.Add(new SqlParameter("@GroupIdx", c.Group.Idx));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Need Group!\n");
                    throw new Exception(message: "Need Group!");
                }
                command.Parameters.Add(new SqlParameter("@Idx", c.Idx));
            }
        }
    }
}