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
    
    public class RequestsDataDB : BaseDB
    {
        public RequestsDataTable SelectAll()
        {
            command.CommandText = $"SELECT * FROM RequestsDataTbl";
            RequestsDataTable pList = new RequestsDataTable(Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            RequestingData p = entity as RequestingData;
            p.Request = ProfileEditRequestsDB.SelectByIdx((int)reader["RequestIdx"]);
            p.Field = reader["Field"].ToString();
            p.OldValue = reader["OldValue"].ToString();
            p.NewValue = reader["NewValue"].ToString();
            base.CreateModel(entity);
            return p;
        }

        protected override BaseEntity NewEntity()
        {
            return new RequestingData();
        }

        static private RequestsDataTable list = new RequestsDataTable();
        public static RequestingData SelectByIdx(int idx)
        {
            RequestsDataDB db = new RequestsDataDB();
            list = db.SelectAll();

            RequestingData g = list.Find(item => item.Idx == idx);
            if (g == null)
            {
                throw new ExpandedException($"RequestingData with Idx {idx} not found.");
            }
            return g;
        }

        //שלב ב
        protected override void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd)
        {
            RequestingData c = entity as RequestingData;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM RequestsDataTbl where Idx=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new SqlParameter("@pid", c.Idx));
            }
        }

        protected override void CreateInsertdSQL(BaseEntity entity, SqlCommand cmd)
        {
            RequestingData c = entity as RequestingData;
            if (c != null)
            {
                string sqlStr = $"INSERT INTO dbo.RequestsDataTbl(RequestIdx, Field, OldValue,NewValue) " +
                        $"VALUES (@RequestIdx, @Field, @OldValue, @NewValue)";
                command.CommandText = sqlStr;

                if (c.Request != null)
                {
                    command.Parameters.Add(new SqlParameter("@RequestIdx", c.Request.Idx));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Need Request!\n");
                    throw new ExpandedException(message: "Need Request!");
                }

                command.Parameters.Add(new SqlParameter("@Field", c.Field));
                command.Parameters.Add(new SqlParameter("@OldValue", c.OldValue));
                command.Parameters.Add(new SqlParameter("@NewValue", c.NewValue));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd)
        {
            RequestingData c = entity as RequestingData;
            if (c != null)
            {
                string sqlStr = $"UPDATE dbo.RequestsDataTbl SET RequestIdx=@RequestIdx, Field=@Field," +
                    $" OldValue=@OldValue, " +
                    $"NewValue=@NewValue WHERE Idx=@Idx";
                cmd.CommandText = sqlStr;

                if (c.Request != null)
                {
                    command.Parameters.Add(new SqlParameter("@RequestIdx", c.Request.Idx));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Need Request!\n");
                    throw new ExpandedException(message: "Need Request!");
                }
                command.Parameters.Add(new SqlParameter("@Field", c.Field));
                command.Parameters.Add(new SqlParameter("@OldValue", c.OldValue));
                command.Parameters.Add(new SqlParameter("@NewValue", c.NewValue));
                cmd.Parameters.Add(new SqlParameter("@Idx", c.Idx));
            }
        }
    }
}