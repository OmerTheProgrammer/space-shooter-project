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
    public class ProfileEditRequestsDB : BaseDB
    {
        public ProfileEditRequestsTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM ProfileEditRequestsTbl ORDER BY idx ASC";
            ProfileEditRequestsTable pList = new ProfileEditRequestsTable(Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            ProfileEditRequest p = entity as ProfileEditRequest;
            DateTime date = new DateTime(1753, 1, 1, 12, 0, 0);
            if (DateTime.TryParse(reader["ReviewingDate"].ToString(), out date))
            {
                p.ReviewingDate = date;
            }
            else
            {
                p.ReviewingDate = null;
            }
            date = new DateTime(1753, 1, 1, 12, 0, 0);
            if (DateTime.TryParse(reader["RequestingDate"].ToString(), out date))
            {
                p.RequestingDate = date;
            }
            p.Status = (Status)(int)reader["Status"];
            p.RequestingPlayer = PlayersDB.SelectByIdx((int)reader["PlayerIdx"]);
            if(reader["AdminIdx"] != DBNull.Value) {
                p.AdressingAdmin = AdminsDB.SelectByIdx((int)reader["AdminIdx"]);
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
        public static ProfileEditRequest SelectByIdx(int idx)
        {
            ProfileEditRequestsDB db = new ProfileEditRequestsDB();
            ProfileEditRequestsTable list = db.SelectAll();

            ProfileEditRequest g = list.Find(item => item.Idx == idx);
            if (g == null)
            {
                throw new ExpandedException($"ProfileEditRequest with Idx {idx} not found.");
            }
            return g;
        }

        //added sql code to replace must check in other PCs
        //works without this code too -> but need to check in API
        //public virtual void Delete(BaseEntity entity)
        //{
        //    BaseEntity reqEntity = this.NewEntity();
        //    if (entity != null)
        //    {
        //        if (entity.GetType() == reqEntity.GetType())
        //        {

        //            RequestsDataDB requestsDataDB = new RequestsDataDB();
        //            RequestsDataTable allRequestingDatas = requestsDataDB.SelectAll();
        //            // Find all RequestingData related to this ProfileEditRequest
        //            List<RequestingData> relatedRequestingDatas = allRequestingDatas.FindAll(item => item.Request.Idx == entity.Idx);
        //            //cast to RequestsDataTable becouse can't in one line
        //            relatedRequestingDatas = relatedRequestingDatas as RequestsDataTable;
        //            if (relatedRequestingDatas != null)
        //            {
        //                foreach (var item in relatedRequestingDatas)
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

        protected override void CreateInsertedSQL(BaseEntity entity, SqlCommand cmd)
        {
            ProfileEditRequest c = entity as ProfileEditRequest;
            if (c != null)
            {

                string sqlStr = $"INSERT INTO dbo.ProfileEditRequestsTbl(PlayerIdx, RequestingDate, Status, ReviewingDate, AdminIdx) " +
                        $"VALUES (@PlayerIdx, @RequestingDate, @Status, @ReviewingDate, @AdminIdx)";
                command.CommandText = sqlStr;

                if (c.RequestingPlayer != null)
                {
                    command.Parameters.Add(new SqlParameter("@PlayerIdx", c.RequestingPlayer.Idx));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Need RequestingPlayer!\n");
                    throw new ExpandedException(message: "Need RequestingPlayer!");
                }
                if (c.AdressingAdmin != null)
                {
                    command.Parameters.Add(new SqlParameter("@AdminIdx", c.AdressingAdmin.Idx));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@AdminIdx", DBNull.Value));
                }
                if(c.RequestingDate != null)
                {
                    command.Parameters.Add(new SqlParameter("@RequestingDate", c.RequestingDate));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@RequestingDate", DBNull.Value));
                }
                if (c.ReviewingDate != null)
                {
                    command.Parameters.Add(new SqlParameter("@ReviewingDate", c.ReviewingDate));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@ReviewingDate", DBNull.Value));
                }
                command.Parameters.Add(new SqlParameter("@Status", (int)c.Status));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd)
        {
            ProfileEditRequest c = entity as ProfileEditRequest;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.ProfileEditRequestsTbl SET PlayerIdx=@PlayerIdx, AdminIdx=@AdminIdx, RequestingDate=@RequestingDate, Status=@Status, " +
                    $"ReviewingDate=@ReviewingDate WHERE Idx=@Idx";

                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new SqlParameter("@PlayerIdx", c.RequestingPlayer.Idx));
                cmd.Parameters.Add(new SqlParameter("@Status", (int)c.Status));
                if (c.RequestingDate != null)
                {
                    command.Parameters.Add(new SqlParameter("@RequestingDate", c.RequestingDate));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@RequestingDate", DBNull.Value));
                }
                if (c.ReviewingDate != null)
                {
                    command.Parameters.Add(new SqlParameter("@ReviewingDate", c.ReviewingDate));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@ReviewingDate", DBNull.Value));
                }
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