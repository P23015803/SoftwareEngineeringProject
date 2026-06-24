using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Group_Project_SIMS
{
    public partial class Results : System.Web.UI.Page
    {
        string connectionString =
        @"Data Source=LAPTOP-0BUOURQ9\SQLEXPRESS;Initial Catalog=SIMS;Integrated Security=True;";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Role verification: Admin only (RoleID == 1)
            if (Session["RoleID"] == null || Convert.ToInt32(Session["RoleID"]) != 1)
            {
                Response.Redirect("Admin Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProgrammes();
                LoadLecturers();
                LoadCourses();
                // default semester list already in markup
                LoadResults();
            }
        }

        protected void ddlProgrammeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // when programme changes, reload courses dropdown to only show courses in selected programme
            int pid;
            if (int.TryParse(ddlProgrammeFilter.SelectedValue, out pid) && pid > 0)
            {
                LoadCourses(pid);
            }
            else
            {
                LoadCourses();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadResults();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = string.Empty;
            ddlSemester.ClearSelection();
            ddlProgrammeFilter.ClearSelection();
            ddlCourseFilter.ClearSelection();
            ddlHOPFilter.ClearSelection();
            ddlLecturerFilter.ClearSelection();
            ddlGradeFilter.ClearSelection();
            ddlPassFailFilter.ClearSelection();
            LoadCourses();
            LoadResults();
        }

        void LoadProgrammes()
        {
            ddlProgrammeFilter.Items.Clear();
            ddlProgrammeFilter.Items.Add(new ListItem("All Programmes", ""));

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string q = "SELECT ProgrammeID, ProgrammeName FROM Programmes";
                using (SqlCommand cmd = new SqlCommand(q, con))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        ddlProgrammeFilter.Items.Add(new ListItem(rdr["ProgrammeName"].ToString(), rdr["ProgrammeID"].ToString()));
                    }
                }
            }
        }

        void LoadLecturers()
        {
            ddlLecturerFilter.Items.Clear();
            ddlLecturerFilter.Items.Add(new ListItem("All Lecturers", ""));

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string q = "SELECT l.LecturerID, u.FullName AS LecturerName FROM Lecturers l INNER JOIN Users u ON l.UserID = u.UserID";
                using (SqlCommand cmd = new SqlCommand(q, con))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        ddlLecturerFilter.Items.Add(new ListItem(rdr["LecturerName"].ToString(), rdr["LecturerID"].ToString()));
                    }
                }
            }
        }

        void LoadCourses(int programmeId = 0)
        {
            ddlCourseFilter.Items.Clear();
            ddlCourseFilter.Items.Add(new ListItem("All Courses", ""));

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string q = "SELECT CourseID, CourseName, ProgrammeID FROM Courses";
                if (programmeId > 0)
                    q += " WHERE ProgrammeID = @pid";

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    if (programmeId > 0)
                        cmd.Parameters.AddWithValue("@pid", programmeId);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ddlCourseFilter.Items.Add(new ListItem(rdr["CourseName"].ToString(), rdr["CourseID"].ToString()));
                        }
                    }
                }
            }
        }

        void LoadResults()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT
                    R.ResultID,
                    E.EnrollmentID,
                    U.UserID,
                    S.StudentID,
                    U.FullName,
                    C.CourseName,
                    E.Semester,
                    P.ProgrammeName,
                    R.Marks,
                    R.Grade,
                    R.GradePoint,
                    S.CGPA,
                    UC.FullName AS CourseLecturerName,
                    UP.FullName AS ProgrammeHOPName,
                    LP.HOPprivileges AS ProgrammeHOPPrivilege
                FROM Results R
                INNER JOIN Enrollments E ON R.EnrollmentID = E.EnrollmentID
                INNER JOIN Students S ON E.StudentID = S.StudentID
                INNER JOIN Users U ON S.UserID = U.UserID
                INNER JOIN Courses C ON E.CourseID = C.CourseID
                LEFT JOIN Lecturers LC ON C.LecturerID = LC.LecturerID
                LEFT JOIN Users UC ON LC.UserID = UC.UserID
                LEFT JOIN Programmes P ON C.ProgrammeID = P.ProgrammeID
                LEFT JOIN Lecturers LP ON P.LecturerID = LP.LecturerID
                LEFT JOIN Users UP ON LP.UserID = UP.UserID
                WHERE 1=1";

                var cmd = new SqlCommand();
                cmd.Connection = conn;

                if (!string.IsNullOrWhiteSpace(txtSearchName.Text))
                {
                    query += " AND U.FullName LIKE @name";
                    cmd.Parameters.AddWithValue("@name", "%" + txtSearchName.Text.Trim() + "%");
                }

                if (!string.IsNullOrWhiteSpace(ddlSemester.SelectedValue))
                {
                    query += " AND E.Semester = @semester";
                    cmd.Parameters.AddWithValue("@semester", ddlSemester.SelectedValue);
                }

                if (!string.IsNullOrWhiteSpace(ddlProgrammeFilter.SelectedValue))
                {
                    query += " AND P.ProgrammeID = @pid";
                    cmd.Parameters.AddWithValue("@pid", Convert.ToInt32(ddlProgrammeFilter.SelectedValue));
                }

                if (!string.IsNullOrWhiteSpace(ddlCourseFilter.SelectedValue))
                {
                    query += " AND E.CourseID = @cid";
                    cmd.Parameters.AddWithValue("@cid", Convert.ToInt32(ddlCourseFilter.SelectedValue));
                }

                if (!string.IsNullOrWhiteSpace(ddlHOPFilter.SelectedValue))
                {
                    // filter by the HOP privilege of the programme's assigned lecturer
                    query += " AND LP.HOPprivileges = @hop";
                    cmd.Parameters.AddWithValue("@hop", Convert.ToInt32(ddlHOPFilter.SelectedValue));
                }

                if (!string.IsNullOrWhiteSpace(ddlGradeFilter.SelectedValue))
                {
                    query += " AND R.Grade = @grade";
                    cmd.Parameters.AddWithValue("@grade", ddlGradeFilter.SelectedValue);
                }

                if (!string.IsNullOrWhiteSpace(ddlPassFailFilter.SelectedValue))
                {
                    // Passing: CGPA >= 2.00 ; Failing: CGPA < 2.00
                    if (ddlPassFailFilter.SelectedValue == "Passing")
                    {
                        query += " AND ISNULL(S.CGPA,0) >= @cgpa";
                        cmd.Parameters.AddWithValue("@cgpa", 2.00m);
                    }
                    else if (ddlPassFailFilter.SelectedValue == "Failing")
                    {
                        query += " AND ISNULL(S.CGPA,0) < @cgpa";
                        cmd.Parameters.AddWithValue("@cgpa", 2.00m);
                    }
                }

                if (!string.IsNullOrWhiteSpace(ddlLecturerFilter.SelectedValue))
                {
                    query += " AND C.LecturerID = @lect";
                    cmd.Parameters.AddWithValue("@lect", Convert.ToInt32(ddlLecturerFilter.SelectedValue));
                }

                cmd.CommandText = query + " ORDER BY U.FullName";

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvResults.DataSource = dt;
                    gvResults.DataBind();

                    if (lblMessage != null)
                        lblMessage.Text = dt.Rows.Count + " results loaded.";
                }
            }
        }
    }
} 