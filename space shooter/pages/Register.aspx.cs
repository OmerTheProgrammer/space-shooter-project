using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace space_shooter.pages
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*protected bool Used_username(string username)
        {
            SQL_Helper.DoProcedure("Space_Shooter_DB.mdf", "Used_name", 
                new Dictionary<string, object>() {
                    { "@username", username } 
                } );

            return false;
        }

        protected bool Used_password(string password)
        {
            SQL_Helper.DoProcedure("Space_Shooter_DB.mdf", "Used_Password",
                new Dictionary<string, object>() {
                    { "@password", password }
                });
            return false;
        }

        //we need to bring the data from JS.
        protected void Submit(string username, string password, string email, string birthday)
        {
            if (Used_username(username))
            {
                Response.Write("msg='this username is taken'");//becouse i'm in script tag
            }
            else if (Used_password(password))
            {
                Response.Write("msg='this password is taken'");//becouse i'm in script tag
            }
            else
            {
                SQL_Helper.DoProcedure("Space_Shooter_DB.mdf", "Insert_User",
                    new Dictionary<string, object>()
                    {
                                        { "@username", username },
                                        { "@password", password },
                                        { "@email", email },
                                        { "@birthday", birthday }
                    }
                );
            }
        }*/
    }
}