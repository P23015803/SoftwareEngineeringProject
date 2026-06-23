using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Group_Project_SIMS
{
    public partial class Enrollment : System.Web.UI.Page
    {
        string connectionString =
        @"Data Source=LAPTOP-0BUOURQ9\SQLEXPRESS;
        Initial Catalog=SIMS;
        Integrated Security=True;";

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
                ViewState["SortDirection"] = "ASC";
                LoadCourses();
                LoadProgrammes();

                // Do not apply any status filter by default (show all) to match "no filtering by default" behavior
                if (ddlStatusFilter != null)
                {
                    ddlStatusFilter.ClearSelection();
                    // ensure the first (All Statuses) is selected if present
                    if (ddlStatusFilter.Items.Count > 0)
                        ddlStatusFilter.Items[0].Selected = true;
                }

                LoadEnrollments(string.Empty);
            }
        } 

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadEnrollments(txtSearchName.Text.Trim());
        }

        protected void ToggleSort_Click(object sender, EventArgs e)
        {
            string cur = (ViewState["SortDirection"] ?? "ASC").ToString();
            string next = cur == "ASC" ? "DESC" : "ASC";
            ViewState["SortDirection"] = next;

            if (btnToggleSort != null)
            {
                btnToggleSort.Text = next == "ASC" ? "Sort ↑" : "Sort ↓";
            }

            LoadEnrollments(txtSearchName.Text.Trim());
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = string.Empty;
            if (ddlCourseFilter != null) ddlCourseFilter.ClearSelection();
            if (ddlProgrammeFilter != null) ddlProgrammeFilter.ClearSelection();
            if (ddlStatusFilter != null) ddlStatusFilter.ClearSelection();

            LoadEnrollments(string.Empty);
        }

        protected void gvEnrollments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Accept" && e.CommandName != "Reject") return;

            int enrollmentID;
            if (!int.TryParse(e.CommandArgument.ToString(), out enrollmentID)) return;

            string newStatus = e.CommandName == "Accept" ? "Enrolled" : "Rejected";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Enrollments SET EnrollmentStatus=@Status WHERE EnrollmentID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@ID", enrollmentID);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        lblMessage.Text = "Enrollment " + enrollmentID + " updated to " + newStatus + ".";
                    }
                    else
                    {
                        lblMessage.Text = "No rows updated.";
                    }
                }
            }

            LoadEnrollments(txtSearchName.Text.Trim());
        }

        private void LoadEnrollments(string searchName)
        {
            string sortDir = (ViewState["SortDirection"] ?? "ASC").ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT
                    E.EnrollmentID,
                    U.UserID,
                    U.FullName,
                    U.Email,
                    S.StudentID,
                    E.CourseID,
                    C.CourseName,
                    C.CourseCode,
                    P.ProgrammeName,
                    P.ProgrammeCode,
                    S.IntakeYear,
                    E.EnrollmentStatus
                FROM Enrollments E
                INNER JOIN Students S ON E.StudentID = S.StudentID
                INNER JOIN Users U ON S.UserID = U.UserID
                INNER JOIN Courses C ON E.CourseID = C.CourseID
                INNER JOIN Programmes P ON C.ProgrammeID = P.ProgrammeID
                WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(searchName))
                {
                    query += " AND U.FullName LIKE @Search";
                }

                if (ddlCourseFilter != null && !string.IsNullOrWhiteSpace(ddlCourseFilter.SelectedValue))
                {
                    query += " AND E.CourseID = @CourseID";
                }

                if (ddlProgrammeFilter != null && !string.IsNullOrWhiteSpace(ddlProgrammeFilter.SelectedValue))
                {
                    query += " AND C.ProgrammeID = @ProgrammeID";
                }

                if (ddlStatusFilter != null && !string.IsNullOrWhiteSpace(ddlStatusFilter.SelectedValue))
                {
                    query += " AND E.EnrollmentStatus = @Status";
                }

                query += " ORDER BY E.EnrollmentID " + (sortDir == "DESC" ? "DESC" : "ASC");

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(searchName))
                        cmd.Parameters.AddWithValue("@Search", "%" + searchName + "%");

                    if (ddlCourseFilter != null && !string.IsNullOrWhiteSpace(ddlCourseFilter.SelectedValue))
                        cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(ddlCourseFilter.SelectedValue));

                    if (ddlProgrammeFilter != null && !string.IsNullOrWhiteSpace(ddlProgrammeFilter.SelectedValue))
                        cmd.Parameters.AddWithValue("@ProgrammeID", Convert.ToInt32(ddlProgrammeFilter.SelectedValue));

                    if (ddlStatusFilter != null && !string.IsNullOrWhiteSpace(ddlStatusFilter.SelectedValue))
                        cmd.Parameters.AddWithValue("@Status", ddlStatusFilter.SelectedValue);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvEnrollments.DataSource = dt;
                        gvEnrollments.DataBind();
                        // helpful debug info: show count of loaded rows
                        if (lblMessage != null)
                        {
                            lblMessage.Text = dt.Rows.Count + " enrollments loaded.";
                        }
                    }
                }
            }
        }

        private void LoadCourses()
        {
            if (ddlCourseFilter == null) return;
            ddlCourseFilter.Items.Clear();
            ddlCourseFilter.Items.Add(new ListItem("All Courses", ""));

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string q = "SELECT CourseID, CourseName FROM Courses";
                using (SqlCommand cmd = new SqlCommand(q, conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        ddlCourseFilter.Items.Add(new ListItem(rdr["CourseName"].ToString(), rdr["CourseID"].ToString()));
                    }
                }
            }
        }

        private void LoadProgrammes()
        {
            if (ddlProgrammeFilter == null) return;
            ddlProgrammeFilter.Items.Clear();
            ddlProgrammeFilter.Items.Add(new ListItem("All Programmes", ""));

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string q = "SELECT ProgrammeID, ProgrammeName FROM Programmes";
                using (SqlCommand cmd = new SqlCommand(q, conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        ddlProgrammeFilter.Items.Add(new ListItem(rdr["ProgrammeName"].ToString(), rdr["ProgrammeID"].ToString()));
                    }
                }
            }
        }

    }
}