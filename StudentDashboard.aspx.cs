using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SoftwareEngineeringProject
{
    public partial class StudentDashboard : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RoleID"] == null || Convert.ToInt32(Session["RoleID"]) != 3)
            {
                Response.Redirect("StudentLogin.aspx");
            }

            if (!IsPostBack)
            {
                LoadProfile();
                LoadCourses();
            }
        }

        private void LoadProfile()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FullName, Email FROM Users WHERE UserID=@UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblName.Text = "Name: " + dr["FullName"].ToString();
                    lblEmail.Text = "Email: " + dr["Email"].ToString();
                }
            }
        }

        private void LoadCourses()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT C.CourseCode, C.CourseName, E.Semester, E.AcademicYear
                                 FROM Enrollments E
                                 INNER JOIN Students S ON E.StudentID = S.StudentID
                                 INNER JOIN Courses C ON E.CourseID = C.CourseID
                                 WHERE S.UserID = @UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("StudentLogin.aspx");
        }
    }
}
