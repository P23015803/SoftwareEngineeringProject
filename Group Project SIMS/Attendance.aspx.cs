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
    public partial class Attendance : System.Web.UI.Page
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
                LoadAttendance();
            }


        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = string.Empty;
            txtDate.Text = string.Empty;
            ddlProgrammeFilter.ClearSelection();
            if (ddlStatusFilter != null) ddlStatusFilter.ClearSelection();
            LoadAttendance();
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

        void LoadAttendance()
        {
            DateTime date;
            bool hasDate = DateTime.TryParse(txtDate.Text, out date);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT
                    A.AttendanceID,
                    E.EnrollmentID,
                    U.UserID,
                    S.StudentID,
                    U.FullName,
                    P.ProgrammeName,
                    UP.FullName AS ProgrammeHOPName,
                    A.AttendanceDate,
                    A.Status
                FROM Attendance A
                INNER JOIN Enrollments E ON A.EnrollmentID = E.EnrollmentID
                INNER JOIN Students S ON E.StudentID = S.StudentID
                INNER JOIN Users U ON S.UserID = U.UserID
                LEFT JOIN Programmes P ON S.ProgrammeID = P.ProgrammeID
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

                if (hasDate)
                {
                    query += " AND CONVERT(date, A.AttendanceDate) = @date";
                    cmd.Parameters.AddWithValue("@date", date.Date);
                }

                if (!string.IsNullOrWhiteSpace(ddlProgrammeFilter.SelectedValue))
                {
                    query += " AND P.ProgrammeID = @pid";
                    cmd.Parameters.AddWithValue("@pid", Convert.ToInt32(ddlProgrammeFilter.SelectedValue));
                }

                if (!string.IsNullOrWhiteSpace(ddlStatusFilter.SelectedValue))
                {
                    query += " AND A.Status = @status";
                    cmd.Parameters.AddWithValue("@status", ddlStatusFilter.SelectedValue);
                }

                cmd.CommandText = query + " ORDER BY A.AttendanceDate DESC, U.FullName";

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvAttendance.DataSource = dt;
                    gvAttendance.DataBind();

                    if (lblMessage != null)
                        lblMessage.Text = dt.Rows.Count + " records loaded.";
                }
            }
        }
    }
}