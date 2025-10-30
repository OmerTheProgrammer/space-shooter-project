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
    public class ProfileEditRequestsDB : BaseDB
    {
        public ProfileEditRequestsTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM ProfileEditRequestsTbl";
            ProfileEditRequestsTable pList = new ProfileEditRequestsTable(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            ProfileEditRequest p = entity as ProfileEditRequest;
            DateTime date = new DateTime(1753, 1, 1, 12, 0, 0);
            if (DateTime.TryParse(reader["ReviewDate"].ToString(), out date))
            {
                p.ReviewDate = date;
            }
            date = new DateTime(1753, 1, 1, 12, 0, 0);
            if (DateTime.TryParse(reader["RequestDate"].ToString(), out date))
            {
                p.ReviewDate = date;
            }
            p.Status = (Status)((int)reader["Status"]);
            p.RequestingPlayer = PlayersDB.SelectById((int)reader["PlayerIdx"]);
            if(reader["AdminIdx"] != DBNull.Value) {
                p.AdressingAdmin = AdminsDB.SelectById((int)reader["AdminIdx"]);
            }
            else
            {
                p.AdressingAdmin = null;
            }
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new ProfileEditRequest();
        }

        static private ProfileEditRequestsTable list = new ProfileEditRequestsTable();
        public static ProfileEditRequest SelectById(int id)
        {
            ProfileEditRequestsDB db = new ProfileEditRequestsDB();
            list = db.SelectAll();

            ProfileEditRequest g = list.Find(item => (item.Idx == id));
            return g;
        }

        //שלב ב
        //added sql code to replace must check in other PCs
        //public virtual void Delete(BaseEntity entity)
        //{
        //    BaseEntity reqEntity = this.NewEntity();
        //    if (entity != null)
        //    {
        //        if (entity.GetType() == reqEntity.GetType())
        //        {

        //            RequestsDataDB requestsDataDB = new RequestsDataDB();
        //            RequestsDataTable allRequestDatas = requestsDataDB.SelectAll();
        //            // Find all RequestData related to this ProfileEditRequest
        //            List<RequestData> relatedRequestDatas = allRequestDatas.FindAll(item => item.Request.Idx == entity.Idx);
        //            //cast to RequestsDataTable becouse can't in one line
        //            relatedRequestDatas = relatedRequestDatas as RequestsDataTable;
        //            if (relatedRequestDatas != null)
        //            {
        //                foreach (var item in relatedRequestDatas)
        //                {
        //                    requestsDataDB.Delete(item);
        //                }
        //            }
        //            deleted.Add(new ChangeEntity(this.CreateDeletedSQL, entity));
        //        }
        //    }
        //}

        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd)
        {
            ProfileEditRequest c = entity as ProfileEditRequest;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM ProfileEditRequestsTbl where Idx=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@pid", c.Idx));
            }
        }

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand cmd)
        {
            ProfileEditRequest c = entity as ProfileEditRequest;
            if (c != null)
            {

                string sqlStr = $"INSERT INTO dbo.ProfileEditRequestsTbl(PlayerIdx, RequestDate, Status, ReviewDate, AdminIdx) " +
                        $"VALUES (@PlayerIdx, @RequestDate, @Status, @ReviewDate, @AdminIdx)";
                command.CommandText = sqlStr;

                if (c.RequestingPlayer != null)
                {
                    command.Parameters.Add(new SqlParameter("@PlayerIdx", c.RequestingPlayer.Idx));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Need RequestingPlayer!\n");
                    throw new Exception(message: "Need RequestingPlayer!");
                }
                if (c.AdressingAdmin != null)
                {
                    command.Parameters.Add(new SqlParameter("@AdminIdx", c.AdressingAdmin.Idx));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@AdminIdx", DBNull.Value));
                }
                command.Parameters.Add(new SqlParameter("@RequestDate", c.RequestDate));
                command.Parameters.Add(new SqlParameter("@Status", (int)c.Status));
                command.Parameters.Add(new SqlParameter("@ReviewDate", c.ReviewDate));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd)
        {
            ProfileEditRequest c = entity as ProfileEditRequest;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.ProfileEditRequestsTbl SET PlayerIdx=@PlayerIdx, RequestDate=@RequestDate, Status=@Status, " +
                    $"ReviewDate=@ReviewDate WHERE Idx=@Idx";
                cmd.CommandText = sqlStr;

                cmd.Parameters.Add(new SqlParameter("@PlayerIdx", c.RequestingPlayer.Idx));
                cmd.Parameters.Add(new SqlParameter("@RequestDate", c.RequestDate));
                cmd.Parameters.Add(new SqlParameter("@Status", (int)c.Status));
                cmd.Parameters.Add(new SqlParameter("@ReviewDate", c.ReviewDate));
                if (c.AdressingAdmin != null)
                {
                    command.Parameters.Add(new SqlParameter("@AdminIdx", c.AdressingAdmin.Idx));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@AdminIdx", DBNull.Value));
                }
                cmd.Parameters.Add(new SqlParameter("@Idx", c.Idx));

            }
        }
    }
}