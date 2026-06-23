using System;
using System.Data.SqlClient;

namespace Group_Project_SIMS
{
    public partial class Login : System.Web.UI.Page
    {
        // Add declarations for server controls referenced in code-behind.
        // These are normally generated in the designer file; adding them here fixes CS0103 when the designer is missing or not generated.
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.TextBox txtPassword;
        protected global::System.Web.UI.WebControls.Label lblMessage;
        protected global::System.Web.UI.WebControls.Button btnLogin;

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            string connectionString =
                @"Server=LAPTOP-0BUOURQ9\SQLEXPRESS;
                Database=SIMS;
                Trusted_Connection=True;";

            using (SqlConnection conn =
                   new SqlConnection(connectionString))
            {
                conn.Open();

                string query =
                @"SELECT UserID,
                         FullName,
                         RoleID
                  FROM Users
                  WHERE Email=@Email
                  AND PasswordHash=@Password";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@Email", email);

                cmd.Parameters.AddWithValue(
                    "@Password", password);

                SqlDataReader reader =
                    cmd.ExecuteReader();

                if (reader.Read())
                {
                    int userID =
                        Convert.ToInt32(reader["UserID"]);

                    int roleID =
                        Convert.ToInt32(reader["RoleID"]);

                    Session["UserID"] = userID;
                    Session["RoleID"] = roleID;

                    switch (roleID)
                    {
                        case 1:
                            Response.Redirect("AdminDash.aspx");
                            break;

                        case 2:
                            Response.Redirect("LecturerDash.aspx");
                            break;

                        case 3:
                            Response.Redirect("StudentDash.aspx");
                            break;

                        default:
                            lblMessage.Text =
                            "Invalid role configuration.";
                            break;
                    }
                }
            }
        }
    }
} 