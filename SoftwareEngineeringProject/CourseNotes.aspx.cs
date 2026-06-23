using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace SoftwareEngineeringProject
{
    public partial class CourseNotes : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;
        int courseID;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Protect page (must be logged in as a lecturer)
            if (Session["LecturerID"] == null)
            {
                Response.Redirect("LecturerLogin.aspx");
                return;
            }

            // Redirect back if no course is active in the dashboard session
            if (Session["SelectedCourseID"] == null)
            {
                Response.Redirect("LecturerDashboard.aspx");
                return;
            }

            courseID = Convert.ToInt32(Session["SelectedCourseID"]);

            if (!IsPostBack)
            {
                LoadCourseName();
                LoadNotes();
            }
        }

        void LoadCourseName()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT CourseName FROM Courses WHERE CourseID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CourseID", courseID);

                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    lblCourseName.Text = result.ToString();
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = Path.GetFileName(FileUpload1.FileName);
                string folderPath = Server.MapPath("~/Notes/");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string savePath = folderPath + fileName;
                FileUpload1.SaveAs(savePath);

                string dbPath = "~/Notes/" + fileName;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"INSERT INTO Notes (LecturerID, CourseID, WeekNo, FileName, FilePath, UploadDate)
                                     VALUES (@LecturerID, @CourseID, @WeekNo, @FileName, @FilePath, @UploadDate)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@LecturerID", Session["LecturerID"]);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    cmd.Parameters.AddWithValue("@WeekNo", ddlWeek.SelectedValue);
                    cmd.Parameters.AddWithValue("@FileName", fileName);
                    cmd.Parameters.AddWithValue("@FilePath", dbPath);
                    cmd.Parameters.AddWithValue("@UploadDate", DateTime.Now);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMessage.Text = "Notes Uploaded Successfully";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                LoadNotes();
            }
        }

        void LoadNotes()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT WeekNo, FileName, FilePath, UploadDate FROM Notes WHERE CourseID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CourseID", courseID);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                gvNotes.DataSource = dt;
                gvNotes.DataBind();
            }
        }
    }
}