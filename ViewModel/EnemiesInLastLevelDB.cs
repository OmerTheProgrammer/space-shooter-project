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
            p.RunInfo = RunsInfoDB.SelectById(int.Parse(reader["RunInfoIdx"].ToString()));
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new EnemyInLastLevel();
        }

        static private EnemiesInLastLevelTable list = new EnemiesInLastLevelTable();
        public static EnemyInLastLevel SelectById(int id)
        {
            EnemiesInLastLevelDB db = new EnemiesInLastLevelDB();
            list = db.SelectAll();

            EnemyInLastLevel g = list.Find(item => (item.Idx == id));
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
                string sqlStr = $"INSERT INTO dbo.EnemiesInLastLevelTbl(Name,Amount,RunIdx) " +
                        $"VALUES (@Name,@Amount,@RunIdx)";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@Amount", c.Amount));
                command.Parameters.Add(new SqlParameter("@Name", c.Name));
                command.Parameters.Add(new SqlParameter("@RunIdx", c.RunInfo.Idx));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand command)
        {
            EnemyInLastLevel c = entity as EnemyInLastLevel;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.EnemiesInLastLevelTbl SET Amount=@Amount," +
                    $"Name=@Name,RunIdx=@RunIdx WHERE Idx=@Idx";
                command.CommandText = sqlStr;

                command.Parameters.Add(new SqlParameter("@Amount", c.Amount));
                command.Parameters.Add(new SqlParameter("@Name", c.Name));
                command.Parameters.Add(new SqlParameter("@RunIdx", c.RunInfo.Idx));
                command.Parameters.Add(new SqlParameter("@Idx", c.Idx));
            }
        }
    }
}