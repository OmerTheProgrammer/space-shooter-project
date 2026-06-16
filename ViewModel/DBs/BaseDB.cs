using Microsoft.Data.SqlClient;
using Model.Entitys;
using System.Data;

namespace ViewModel.DBs
{
    public abstract class BaseDB
    {
        protected string connectionString = GetConnectionString();
        protected static SqlConnection connection;
        protected static SqlTransaction trans = null;
        protected SqlCommand command;
        protected SqlDataReader reader;
        protected List<ChangeEntity> changes = new List<ChangeEntity>();

        private static string GetConnectionString()
        {
            // 1. Get the directory of the executing assembly (e.g., bin/Debug/net8.0/)
            string assemblyPath = Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            // 2. Navigate UP to the project root (assuming standard structure)
            // This might need adjustment based on your specific solution structure (e.g., 3 levels up: \bin\Debug\net8.0\)
            // Let's assume the DB file is in a known location relative to the solution file.
            // A more robust approach: Find the project path.

            // This navigates up three levels from the running DLL:
            // ViewModel.dll <- bin <- Debug <- net8.0 (1) <- ProjectName (2) <- SolutionFolder (3)
            string projectRoot = Path.GetFullPath(
                Path.Combine(assemblyPath, @"..\..\..\.."));

            // Assuming your .mdf file is located in the root of your ViewModel project folder:
            string dbFilePath = Path.Combine(
                projectRoot, "ViewModel", "Space_Shooter_DB.mdf");

            // 3. Construct the connection string using the correct, fully qualified path
            return "Data Source=(LocalDB)\\MSSQLLocalDB;" +
                   "AttachDbFilename=\"" + dbFilePath + "\";" +
                   "Integrated Security=True;" +
                   "Connect Timeout=30;" +
                   "MultipleActiveResultSets=True;";
        }

        public BaseDB()
        {
            connection ??= new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
        }

        protected abstract BaseEntity NewEntity();

        protected List<BaseEntity> Select()
        {
            List<BaseEntity> list = new List<BaseEntity>();
            try
            {
                command.Connection = connection;
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                //in order to run while a transaction (other commend - insert etc) is active, otherwise
                //the connection will be locked and the select will fail.
                if (trans != null)//אם יש טרנזקציה פעילה, נשתמש בה
                {
                    command.Transaction = trans;
                }
                else//אם אין טרנזקציה פעילה, נבטל את הטרנזקציה של הפקודה כדי לאפשר את הריצה שלה
                {
                    command.Transaction = null;
                }

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BaseEntity entity = NewEntity();
                    list.Add(CreateModel(entity));
                }
            }
            catch (Exception e)
            {
                throw new ExpandedException("\nSQL:", command.CommandText, e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }
        protected virtual BaseEntity CreateModel(BaseEntity entity)
        {
            entity.Idx = (int)reader["Idx"];
            return entity;
        }

        protected abstract void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd);

        protected virtual void CreateDeletedFatherSQL(BaseEntity entity, SqlCommand cmd)
        {
            //יורש ישים פה: base.CreateDeletedSQL(entity, cmd);
        }

        /// <summary>
        /// Deletes a record based on a *FOUND* idx in the table!
        /// </summary>
        /// <param name="entity">the entity to delete *WITH* his Idx</param>
        /// <returns>Nothing</returns>
        /// <remarks>
        /// starts the delete process.
        /// </remarks>
        public virtual void Delete(BaseEntity entity)
        {
            if (entity != null)
            {
                changes.Add(new ChangeEntity(entity, DbAction.Delete));
            }
        }

        protected abstract void CreateInsertedSQL(BaseEntity entity, SqlCommand cmd);
        protected virtual void CreateInsertedFatherSQL(BaseEntity entity, SqlCommand cmd)
        {
            //יורש ישים פה: base.CreateInsertedSQL(entity, cmd);
        }

        /// <summary>
        /// Inserts a record into the table!
        /// </summary>
        /// <param name="entity">the entity to insert (no idx)</param>
        /// <returns>Nothing</returns>
        /// <remarks>
        /// starts the insert process.
        /// </remarks>
        public virtual void Insert(BaseEntity entity)
        {
            if (entity != null)
            {
                changes.Add(new ChangeEntity(entity, DbAction.Insert));
            }
        }

        protected abstract void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd);
        protected virtual void CreateUpdatedFatherSQL(BaseEntity entity, SqlCommand cmd)
        {
            //יורש ישים פה: base.CreateUpdatedSQL(entity, cmd);
        }

        /// <summary>
        /// Updates a record based on a *FOUND* idx in the table!
        /// </summary>
        /// <param name="entity">the entity to update *WITH* his Idx</param>
        /// <returns>Nothing</returns>
        /// <remarks>
        /// starts the update process.
        /// </remarks>
        public virtual void Update(BaseEntity entity)
        {
            if (entity != null)
            {
                changes.Add(new ChangeEntity(entity, DbAction.Update));
            }
        }

        /// <summary>
        /// actully CHANGES the DB, does the actul inserting/updating/deleting
        /// </summary>
        /// <returns>amount of lines he changed</returns>
        /// <remarks>
        /// finishes the update/insert/delete processes.
        /// </remarks>
        public int SaveChanges()
        {
            trans = null;
            int records_affected = 0;

            try
            {
                command.Connection = connection;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                trans = connection.BeginTransaction();
                command.Transaction = trans;

                // לולאה אחת ברורה על כל השינויים שנאספו
                foreach (var change in changes)
                {
                    command.Parameters.Clear();

                    switch (change.Action)
                    {
                        case DbAction.Insert:
                            CreateInsertedSQL(change.Entity, command);
                            records_affected += command.ExecuteNonQuery();

                            command.CommandText = "SELECT @@IDENTITY";
                            object result = command.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                change.Entity.Idx = Convert.ToInt32(result);
                            }

                            break;

                        case DbAction.InsertChild: // האב (Users) רץ ראשון ומייצר את ה-ID!
                            CreateInsertedSQL(change.Entity, command);
                            records_affected += command.ExecuteNonQuery();
                            break;

                        case DbAction.InsertFather: // האב (Users) רץ ראשון ומייצר את ה-ID!
                            CreateInsertedFatherSQL(change.Entity, command);
                            records_affected += command.ExecuteNonQuery();

                            // שליפת ה-ID האוטומטי שנוצר באב ועדכון הישות מיד!
                            command.CommandText = "SELECT @@IDENTITY";
                            object resultFather = command.ExecuteScalar();
                            if (resultFather != null && resultFather != DBNull.Value)
                            {
                                change.Entity.Idx = Convert.ToInt32(resultFather);
                            }
                            break;

                        case DbAction.Update:
                            CreateUpdatedSQL(change.Entity, command);
                            records_affected += command.ExecuteNonQuery();
                            break;

                        case DbAction.UpdateFather: // מריץ את שאילתת העדכון של האב
                            CreateUpdatedFatherSQL(change.Entity, command);
                            records_affected += command.ExecuteNonQuery();
                            break;

                        case DbAction.Delete:
                            CreateDeletedSQL(change.Entity, command);
                            records_affected += command.ExecuteNonQuery();
                            break;

                        case DbAction.DeleteFather://runs base.CreateDeletedSQL for inherited.
                            CreateDeletedFatherSQL(change.Entity, command);
                            records_affected += command.ExecuteNonQuery();
                            break;
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans?.Rollback();
                throw new ExpandedException("Sql error happened: ", command.CommandText, ex);
            }
            finally
            {
                // מנקים את רשימת השינויים שבוצעו בהצלחה
                changes.Clear();
            }

            return records_affected;
        }

    }
}

