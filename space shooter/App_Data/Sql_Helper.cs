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


public class SQL_Helper
{
    public static SqlConnection openConnection(string connString)
    {
        SqlConnection copy_conn = null;
        try
        {
            SqlConnection conn = new SqlConnection(connString);
            if (conn.State == ConnectionState.Closed) { 
                conn.Open();
                copy_conn = conn;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return copy_conn;
    }

    public static SqlConnection ConnectToDb(string fileName)
    {
        //HttpContext.Current.Server.MapPath(fileName);
        string path = "C:\\Users\\omer1\\OneDrive\\שולחן העבודה\\programing\\my_games\\to_computer\\games_in_cs\\space shooter project\\space shooter\\App_Data\\" + fileName;

        //string connString = @"Data Source=.\SQLEXPRESS;AttachDbFileName=" + path + ";Integrated Security=True;User Instance=True";
        //string connString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\" + fileName + " Integrated Security = True";
        //string connString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + path + " Integrated Security = True";
        //string connString = @"";
        string connString = 
            @"Data Source=(LocalDB)\MSSQLLocalDB;
            AttachDbFilename=" + path + ";" +
            " Integrated Security = True;" +
            "Connect Timeout = 30";

        return openConnection(connString);
    }

    public static void DoQuery(string fileName, string sql, Dictionary<string, object> parameters)
	{
        SqlConnection conn = ConnectToDb(fileName);

        SqlCommand com = new SqlCommand(sql, conn);

        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                com.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
        }

        com.ExecuteNonQuery();
        conn.Close();
    }

    public static void DoProcedure(string fileName, string sql, Dictionary<string, object> parameters)
    {
        SqlConnection conn = ConnectToDb(fileName);

        SqlCommand com = new SqlCommand(sql, conn);
        com.CommandType = CommandType.StoredProcedure;

        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                com.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
        }

        com.ExecuteNonQuery();
        conn.Close();
    }

    public static bool IsExist(string fileName, string sql)
	{

		SqlConnection conn = ConnectToDb(fileName);

		SqlCommand com = new SqlCommand(sql, conn);
		SqlDataReader data = com.ExecuteReader();

		bool found = Convert.ToBoolean(data.Read());
		conn.Close();
		return found;
	}

    public static DataTable ExecuteDataTable(string fileName, string sql)
    {
        SqlConnection conn = ConnectToDb(fileName);

        DataTable dt = new DataTable();

        SqlDataAdapter tableAdapter = new SqlDataAdapter(sql, conn);

        tableAdapter.Fill(dt);

        return dt;
    }

}