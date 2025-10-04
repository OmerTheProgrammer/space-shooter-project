using Microsoft.Data.SqlClient;
using Model.Entitys;
using System.Data;

namespace ViewModel
{
    public abstract class BaseDB
    {
        protected static string connectionString = GetConnectionString();

        protected static SqlConnection connection;
        protected SqlCommand command;
        protected SqlDataReader reader;

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
                   "Connect Timeout=30;";
        }

        public BaseDB()
        {
            connection ??= new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
        }

        public abstract BaseEntity NewEntity();

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

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BaseEntity entity = NewEntity();
                    list.Add(CreateModel(entity));
                }
            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine(
                    e.Message + "\nSQL:" + command.CommandText);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection.State == ConnectionState.Open) //גורם לקריסה?
                {
                    connection.Close();
                }
            }
            return list;
        }

        protected async Task<List<BaseEntity>> SelectAsync(string sqlStr)
        {
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();
            List<BaseEntity> list = new List<BaseEntity>();

            try
            {
                command.Connection = connection;
                command.CommandText = sqlStr;
                connection.Open();
                this.reader = (SqlDataReader)await command.ExecuteReaderAsync();


                while (reader.Read())
                {
                    BaseEntity entity = NewEntity();
                    list.Add(CreateModel(entity));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\nSQL:" + command.CommandText);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return list;
        }


        protected virtual BaseEntity CreateModel(BaseEntity entity)
        {
            entity.Idx = (int)reader["Idx"];
            return entity;
        }

        protected abstract void CreateDeletedSQL(BaseEntity entity, SqlCommand cmd);
        public static List<ChangeEntity> deleted = new List<ChangeEntity>();

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
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null)
            {
                if (entity.GetType() == reqEntity.GetType())
                {
                    deleted.Add(new ChangeEntity(this.CreateDeletedSQL, entity));
                }
            }
        }

        protected abstract void CreateInsertdSQL(BaseEntity entity, SqlCommand cmd);
        public static List<ChangeEntity> inserted = new List<ChangeEntity>();

        /// <summary>
        /// Inserts a record inyo the table!
        /// </summary>
        /// <param name="entity">the entity to insert (no idx)</param>
        /// <returns>Nothing</returns>
        /// <remarks>
        /// starts the insert process.
        /// </remarks>
        public virtual void Insert(BaseEntity entity)
        {
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null & entity.GetType() == reqEntity.GetType())
            {
                inserted.Add(new ChangeEntity(this.CreateInsertdSQL, entity));
            }
        }

        protected abstract void CreateUpdatedSQL(BaseEntity entity, SqlCommand cmd);
        public static List<ChangeEntity> updated = new List<ChangeEntity>();

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
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                updated.Add(new ChangeEntity(this.CreateUpdatedSQL, entity));
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
            SqlTransaction trans = null;
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

                foreach (var entity in inserted)
                {
                    command.Parameters.Clear();
                    entity.CreateSql(entity.Entity, command); //cmd.CommandText = CreateInsertSQL(entity.Entity);
                    records_affected += command.ExecuteNonQuery();

                    command.CommandText = "SELECT SCOPE_IDENTITY()";

                    object result = command.ExecuteScalar();
                    if(result != null && result.GetType().ToString() != "System.DBNull")
                    {
                        entity.Entity.Idx = Convert.ToInt32(result);
                    }
                }

                foreach (var entity in updated)
                {
                    command.Parameters.Clear();
                    entity.CreateSql(entity.Entity, command);        //cmd.CommandText = CreateUpdateSQL(entity.Entity);
                    records_affected += command.ExecuteNonQuery();
                }

                foreach (var entity in deleted)
                {
                    command.Parameters.Clear();
                    entity.CreateSql(entity.Entity, command);

                    records_affected += command.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                System.Diagnostics.Debug.WriteLine(ex.Message + "\n SQL:" + command.CommandText);
            }
            finally
            {
                inserted.Clear();

                updated.Clear();

                deleted.Clear();

                if (connection.State == System.Data.ConnectionState.Open)
                {//גורם לקריסה?
                    connection.Close();
                }
            }

            return records_affected;
        }

    }
}

