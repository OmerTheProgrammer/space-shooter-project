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
    public class RunsInfoDB : BaseDB
    {
        public RunsInfoTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM RunsInfoTbl";
            RunsInfoTable pList = new RunsInfoTable(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            RunInfo p = entity as RunInfo;
            p.Player = PlayersDB.SelectByIdx(int.Parse(reader["PlayerIdx"].ToString()));
            p.CurrentScore = int.Parse(reader["CurrentScore"].ToString());
            p.CurrentLevel = int.Parse(reader["CurrentLevel"].ToString());
            p.RunStopDate = DateTime.Parse(reader["RunStopDate"].ToString());
            p.CurrentShieldLevel = int.Parse(reader["CurrentShieldLevel"].ToString());
            p.CurrentBlasterCount = int.Parse(reader["CurrentBlasterCount"].ToString());
            p.CurrentHp = int.Parse(reader["CurrentHp"].ToString());
            p.IsRunOver = bool.Parse(reader["IsRunOver"].ToString());
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new RunInfo();
        }

        static private RunsInfoTable list = new RunsInfoTable();
        public static RunInfo SelectByIdx(int idx)
        {
            RunsInfoDB db = new RunsInfoDB();
            list = db.SelectAll();

            RunInfo g = list.Find(item => (item.Idx == idx));
            if (g == null)
            {
                throw new Exception($"RunInfo with Idx {idx} not found.");
            }
            return g;
        }

        //שלב ב
        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd)
        {
            RunInfo c = entity as RunInfo;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM RunsInfoTbl where Idx=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@pid", c.Idx));
            }
        }

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand cmd)
        {
            RunInfo c = entity as RunInfo;
            if (c != null)
            {

                string sqlStr = $"INSERT INTO dbo.RunsInfoTbl(PlayerIdx, CurrentScore, CurrentLevel" +
                    $", RunStopDate, CurrentShieldLevel, CurrentBlasterCount, CurrentHp," +
                    $"IsRunOver) VALUES (@PlayerIdx, @CurrentScore, @CurrentLevel," +
                    $" @RunStopDate, @CurrentShieldLevel, @CurrentBlasterCount," +
                    $" @CurrentHp, @IsRunOver)";
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
                command.Parameters.Add(new SqlParameter("@CurrentScore", c.CurrentScore));
                command.Parameters.Add(new SqlParameter("@CurrentLevel", c.CurrentLevel));
                command.Parameters.Add(new SqlParameter("@RunStopDate", c.RunStopDate));
                command.Parameters.Add(new SqlParameter("@CurrentShieldLevel", c.CurrentShieldLevel));
                command.Parameters.Add(new SqlParameter("@CurrentBlasterCount", c.CurrentBlasterCount));
                command.Parameters.Add(new SqlParameter("@CurrentHp", c.CurrentHp));
                command.Parameters.Add(new SqlParameter("@IsRunOver", c.IsRunOver));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand command)
        {
            RunInfo c = entity as RunInfo;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.RunsInfoTbl SET PlayerIdx=@PlayerIdx, " +
                    $"CurrentLevel=@CurrentLevel,RunStopDate=@RunStopDate," +
                    $"CurrentShieldLevel=@CurrentShieldLevel," +
                    $"CurrentBlasterCount=@CurrentBlasterCount, " +
                    $"CurrentHp=@CurrentHp,IsRunOver=@IsRunOver WHERE Idx=@Idx";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@PlayerIdx", c.Player.Idx));
                command.Parameters.Add(new SqlParameter("@CurrentScore", c.CurrentScore));
                command.Parameters.Add(new SqlParameter("@CurrentLevel", c.CurrentLevel));
                command.Parameters.Add(new SqlParameter("@RunStopDate", c.RunStopDate));
                command.Parameters.Add(new SqlParameter("@CurrentShieldLevel", c.CurrentShieldLevel));
                command.Parameters.Add(new SqlParameter("@CurrentBlasterCount", c.CurrentBlasterCount));
                command.Parameters.Add(new SqlParameter("@CurrentHp", c.CurrentHp));
                command.Parameters.Add(new SqlParameter("@IsRunOver", c.IsRunOver));
                command.Parameters.Add(new SqlParameter("@Idx", c.Idx));

            }
        }
    }
}