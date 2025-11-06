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
    public class PlayersDB : UsersDB
    {
        public PlayersTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM (PlayersTbl INNER JOIN\r\n UsersTbl ON PlayersTbl.Idx = UsersTbl.Idx)";
            PlayersTable pList = new PlayersTable(base.Select());
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

        static private PlayersTable list = new PlayersTable();
        public static Player SelectByIdx(int idx)
        {
            PlayersDB db = new PlayersDB();
            list = db.SelectAll();

            Player g = list.Find(item => (item.Idx == idx));
            if (g == null)
            {
                throw new Exception($"Player with Idx {idx} not found.");
            }
            return g;
        }

        //שלב ב
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

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand cmd)
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