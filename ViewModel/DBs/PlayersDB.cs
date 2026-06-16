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
    public class PlayersDB : UsersDB
    {
        public PlayersTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM (PlayersTbl INNER JOIN" +
                $"\r\n UsersTbl ON PlayersTbl.Idx = UsersTbl.Idx) ORDER BY UsersTbl.Idx ASC";
            PlayersTable pList = new PlayersTable(Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Player p = entity as Player;
            p.MaxLevel = int.Parse(reader["MaxLevel"].ToString());
            p.TotalScore = int.Parse(reader["TotalScore"].ToString());
            p.IsMusicOn = bool.Parse(reader["IsMusicOn"].ToString());
            p.IsSoundOn = bool.Parse(reader["IsSoundOn"].ToString());
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new Player();
        }
        public static Player SelectByIdx(int idx)
        {
            PlayersDB db = new PlayersDB();
            PlayersTable list = db.SelectAll();

            Player g = list.Find(item => item.Idx == idx);
            if (g == null)
            {
                throw new ExpandedException($"Player with Idx {idx} not found.");
            }
            return g;
        }

        //שלב ב
        public override void Delete(BaseEntity entity)
        {
            if (entity == null) return;
            //delete player and user
            changes.Add(new ChangeEntity(entity, DbAction.DeleteFather));
            changes.Add(new ChangeEntity(entity, DbAction.Delete));
        }

        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd)
        {
            Player c = entity as Player;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM PlayersTbl where idx=@pid";

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
                changes.Add(new ChangeEntity(entity, DbAction.InsertFather));
                changes.Add(new ChangeEntity(entity, DbAction.InsertChild));
            
        }

        protected override void CreateInsertedSQL(BaseEntity entity, SqlCommand cmd)
        {
            Player c = entity as Player;
            if (c != null)
            {
                string sqlStr = $"Insert INTO  PlayersTbl (Idx,MaxLevel,TotalScore," +
                    $"IsMusicOn,IsSoundOn) VALUES " +
                    $"(@idx,@MaxLevel,@TotalScore,@IsMusicOn,@IsSoundOn)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@idx", c.Idx));
                command.Parameters.Add(new SqlParameter("@MaxLevel", c.MaxLevel));
                command.Parameters.Add(new SqlParameter("@TotalScore", c.TotalScore));
                command.Parameters.Add(new SqlParameter("@IsMusicOn", c.IsMusicOn));
                command.Parameters.Add(new SqlParameter("@IsSoundOn", c.IsSoundOn));
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
            Player c = entity as Player;
            if (c != null)
            {
                string sqlStr = $"UPDATE PlayersTbl SET MaxLevel=@MaxLevel," +
                    $"TotalScore=@TotalScore, IsMusicOn=@IsMusicOn, " +
                    $"IsSoundOn = @IsSoundOn WHERE Idx=@idx";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@idx", c.Idx));
                command.Parameters.Add(new SqlParameter("@MaxLevel", c.MaxLevel));
                command.Parameters.Add(new SqlParameter("@TotalScore", c.TotalScore));
                command.Parameters.Add(new SqlParameter("@IsMusicOn", c.IsMusicOn));
                command.Parameters.Add(new SqlParameter("@IsSoundOn", c.IsSoundOn));
            }
        }

        protected override void CreateUpdatedFatherSQL(BaseEntity entity, SqlCommand cmd)
        {
            base.CreateUpdatedSQL(entity, cmd); // קורא ל-SQL של UsersDB
        }
    }
}