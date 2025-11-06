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
    
    public class EnemiesInLastLevelDB : BaseDB
    {
        public EnemiesInLastLevelTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM EnemiesInLastLevelTbl";
            EnemiesInLastLevelTable pList = new EnemiesInLastLevelTable(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            EnemyInLastLevel p = entity as EnemyInLastLevel;
            p.Name = (Enemy)int.Parse(reader["Name"].ToString());
            p.Amount = int.Parse(reader["Amount"].ToString());
            p.RunInfo = RunsInfoDB.SelectByIdx(int.Parse(reader["RunInfoIdx"].ToString()));
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new EnemyInLastLevel();
        }

        static private EnemiesInLastLevelTable list = new EnemiesInLastLevelTable();
        public static EnemyInLastLevel SelectByIdx(int idx)
        {
            EnemiesInLastLevelDB db = new EnemiesInLastLevelDB();
            list = db.SelectAll();

            EnemyInLastLevel g = list.Find(item => (item.Idx == idx));
            if (g == null)
            {
                throw new Exception($"EnemyInLastLevel with Idx {idx} not found.");
            }
            return g;
        }

        //שלב ב

        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand command)
        {
            EnemyInLastLevel c = entity as EnemyInLastLevel;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM EnemiesInLastLevelTbl where Idx=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@pid", c.Idx));
            }
        }

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand command)
        {
            EnemyInLastLevel c = entity as EnemyInLastLevel;
            if (c != null)
            {
                string sqlStr = $"INSERT INTO dbo.EnemiesInLastLevelTbl(Name,Amount,RunInfoIdx) " +
                        $"VALUES (@Name,@Amount,@RunInfoIdx)";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@Amount", c.Amount));
                command.Parameters.Add(new SqlParameter("@Name", c.Name));
                if (c.RunInfo != null)
                {
                    command.Parameters.Add(new SqlParameter("@RunInfoIdx", c.RunInfo.Idx));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Need RunInfo!\n");
                    throw new Exception(message: "Need RunInfo!");
                }
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand command)
        {
            EnemyInLastLevel c = entity as EnemyInLastLevel;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.EnemiesInLastLevelTbl SET Amount=@Amount," +
                    $"Name=@Name,RunInfoIdx=@RunInfoIdx WHERE Idx=@Idx";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@Amount", c.Amount));
                command.Parameters.Add(new SqlParameter("@Name", c.Name));
                command.Parameters.Add(new SqlParameter("@RunInfoIdx", c.RunInfo.Idx));
                command.Parameters.Add(new SqlParameter("@Idx", c.Idx));
            }
        }
    }
}