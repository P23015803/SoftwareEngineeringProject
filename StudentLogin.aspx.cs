using System;
using System.Data.SqlClient;
using System.Configuration;

namespace SoftwareEngineeringProject
{
    public partial class StudentLogin : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT UserID, RoleID 
                                 FROM Users 
                                 WHERE Email=@Email AND PasswordHash=@Password AND RoleID=3";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Session["UserID"] = dr["UserID"];
                    Session["RoleID"] = dr["RoleID"];

                    Response.Redirect("StudentDashboard.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid login.";
                }
            }
        }
    }
}
