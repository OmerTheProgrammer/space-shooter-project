using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Data.Common;
using System.Collections.Generic;
using System.ComponentModel;

/// <summary>
/// Summary description for Helper
/// </summary>
/// 

public class SqlDatabaseManager
{
    public SqlDatabaseManager(string path)
    {
        this.connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True;";
    }
        
    private string connectionString;

    public List<Dictionary<string, object>> ExecuteQuery(string sqlQuery, SqlParameter[] parameters = null)
    {
        List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters); // Add parameters if provided
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader[i]);
                            }
                            results.Add(row);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error executing query: " + ex.Message);
            // Consider logging the exception details for debugging.
            throw; // Re-throw the exception after logging (or handling as needed)
        }

        return results;
    }


    public int ExecuteNonQuery(string sqlQuery, SqlParameter[] parameters = null)
    {
        int rowsAffected = 0;

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error executing non-query: " + ex.Message);
            throw; // Re-throw or handle as needed
        }

        return rowsAffected;
    }
}

//public class SqlDatabaseManager
//{
//    public static SqlConnection openConnection(string connString)
//    {
//        SqlConnection copy_conn = null;
//        try
//        {
//            SqlConnection conn = new SqlConnection(connString);
//            if (conn.State == ConnectionState.Closed) { 
//                conn.Open();
//                copy_conn = conn;
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//        }
//        return copy_conn;
//    }

//    private static string GetConnectionString(string fileName)
//    {
//        string path = HttpContext.Current != null ? // Check if running in web context
//                       HttpContext.Current.Server.MapPath("~/App_Data/" + fileName) : // Use relative path for web
//                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", fileName); // Use relative path for other contexts

//        return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True;Connect Timeout=30";
//    }

//    public static SqlConnection ConnectToDb(string fileName)
//    {
//        string connString = GetConnectionString(fileName);
//        SqlConnection conn = new SqlConnection(connString);
//        if (conn.State == ConnectionState.Closed)
//        {
//            conn.Open();
//        }
//        return conn; // No need for a copy_conn unless you intend to keep the original conn object unchanged.
//    }

//    public static void DoQuery(string fileName, string sql, Dictionary<string, object> parameters)
//	{
//        SqlConnection conn = ConnectToDb(fileName);

//        SqlCommand com = new SqlCommand(sql, conn);

//        if (parameters != null)
//        {
//            foreach (var parameter in parameters)
//            {
//                com.Parameters.AddWithValue(parameter.Key, parameter.Value);
//            }
//        }

//        com.ExecuteNonQuery();
//        conn.Close();
//    }

//    public static void DoProcedure(string fileName, string sql, Dictionary<string, object> parameters)
//    {
//        SqlConnection conn = ConnectToDb(fileName);

//        SqlCommand com = new SqlCommand(sql, conn);
//        com.CommandType = CommandType.StoredProcedure;

//        if (parameters != null)
//        {
//            foreach (var parameter in parameters)
//            {
//                com.Parameters.AddWithValue(parameter.Key, parameter.Value);
//            }
//        }

//        com.ExecuteNonQuery();
//        conn.Close();
//    }

//    public static bool IsExist(string fileName, string sql)
//	{

//		SqlConnection conn = ConnectToDb(fileName);

//		SqlCommand com = new SqlCommand(sql, conn);
//		SqlDataReader data = com.ExecuteReader();

//		bool found = Convert.ToBoolean(data.Read());
//		conn.Close();
//		return found;
//	}

//    public static DataTable ExecuteDataTable(string fileName, string sql)
//    {
//        SqlConnection conn = ConnectToDb(fileName);

//        DataTable dt = new DataTable();

//        SqlDataAdapter tableAdapter = new SqlDataAdapter(sql, conn);

//        tableAdapter.Fill(dt);

//        return dt;
//    }

//}