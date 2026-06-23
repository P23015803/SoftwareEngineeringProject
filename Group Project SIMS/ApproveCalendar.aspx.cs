//remember to fit all the calendar files unto the main system and tweak the database to fit

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace Group_Project_SIMS
{
    public partial class ApproveCalendar : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
            ConfigurationManager.ConnectionStrings["SIMS"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check role by RoleID (Admin = 1)
            if (Session["RoleID"] == null ||
                Convert.ToInt32(Session["RoleID"]) != 1)
            {
                Response.Redirect("Admin Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadEvents();
            }
        }

        private void LoadEvents(string title = null, string type = null, string semester = null)
        {
            try
            {
                string sql = @"SELECT
                    CalendarID,
                    EventTitle,
                    EventDate,
                    EventType,
                    Semester,
                    CreatedBy,
                    Status,
                    PublishStatus
                  FROM AcademicCalendar
                  WHERE 1=1";

                var cmd = new SqlCommand();
                cmd.Connection = con;

                if (!string.IsNullOrWhiteSpace(title))
                {
                    sql += " AND EventTitle LIKE @Title";
                    cmd.Parameters.AddWithValue("@Title", "%" + title.Trim() + "%");
                }

                if (!string.IsNullOrWhiteSpace(type) && type != "All")
                {
                    sql += " AND EventType = @Type";
                    cmd.Parameters.AddWithValue("@Type", type);
                }

                if (!string.IsNullOrWhiteSpace(semester) && semester != "All")
                {
                    sql += " AND Semester = @Semester";
                    cmd.Parameters.AddWithValue("@Semester", semester);
                }

                // Order by CalendarID to ensure predictable ordering
                sql += " ORDER BY CalendarID ASC";

                cmd.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                // map CreatedBy to FullName from Users table
                try
                {
                DataTable users = new DataTable();
                SqlDataAdapter uda = new SqlDataAdapter("SELECT UserID, FullName FROM Users", con);
                uda.Fill(users);

                if (!dt.Columns.Contains("CreatedByName"))
                    dt.Columns.Add("CreatedByName", typeof(string));

                foreach (DataRow r in dt.Rows)
                {
                    string createdBy = r["CreatedBy"] == DBNull.Value ? string.Empty : r["CreatedBy"].ToString();
                    string found = null;

                    foreach (DataRow u in users.Rows)
                    {
                        if (u["UserID"] != DBNull.Value && u["UserID"].ToString() == createdBy)
                        {
                            found = u["FullName"].ToString();
                            break;
                        }
                    }

                    r["CreatedByName"] = string.IsNullOrEmpty(found) ? createdBy : found;
                }
                }
                catch
                {
                    if (!dt.Columns.Contains("CreatedByName"))
                    {
                        dt.Columns.Add("CreatedByName", typeof(string));
                        foreach (DataRow r in dt.Rows)
                            r["CreatedByName"] = r["CreatedBy"] == DBNull.Value ? string.Empty : r["CreatedBy"].ToString();
                    }
                }

                gvEvents.DataSource = dt;
                gvEvents.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string typeFilter = (ddlFilterType != null && ddlFilterType.SelectedIndex > 0) ? ddlFilterType.SelectedValue : null;
            string semesterFilter = (ddlFilterSemester != null && ddlFilterSemester.SelectedIndex > 0) ? ddlFilterSemester.SelectedValue : null;
            LoadEvents(txtSearchTitle.Text, typeFilter, semesterFilter);
        }

        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtSearchTitle.Text = string.Empty;
            if (ddlFilterType.Items.Count > 0) ddlFilterType.SelectedIndex = 0;
            if (ddlFilterSemester.Items.Count > 0) ddlFilterSemester.SelectedIndex = 0;
            LoadEvents();
        }

        protected void gvEvents_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve" ||
                e.CommandName == "Reject" ||
                e.CommandName == "Publish")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                string id =
                    gvEvents.Rows[rowIndex]
                    .Cells[0].Text;

                try
                {
                    con.Open();

                    if (e.CommandName == "Approve")
                    {
                        SqlCommand cmd = new SqlCommand(
                        @"UPDATE AcademicCalendar
                          SET Status='Approved'
                          WHERE CalendarID=@ID", con);

                        cmd.Parameters.AddWithValue("@ID", id);

                        cmd.ExecuteNonQuery();

                        lblMessage.Text =
                        "Event approved successfully.";
                    }

                    else if (e.CommandName == "Reject")
                    {
                        SqlCommand cmd = new SqlCommand(
                        @"UPDATE AcademicCalendar
                          SET Status='Rejected'
                          WHERE CalendarID=@ID", con);

                        cmd.Parameters.AddWithValue("@ID", id);

                        cmd.ExecuteNonQuery();

                        lblMessage.Text =
                        "Event rejected successfully.";
                    }

                    else if (e.CommandName == "Publish")
                    {
                        SqlCommand cmd = new SqlCommand(
                        @"UPDATE AcademicCalendar
                          SET PublishStatus='Published'
                          WHERE CalendarID=@ID
                          AND Status='Approved'", con);

                        cmd.Parameters.AddWithValue("@ID", id);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            lblMessage.Text =
                            "Event published successfully.";
                        }
                        else
                        {
                            lblMessage.Text =
                            "Only approved events can be published.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
                finally
                {
                    con.Close();
                }

                LoadEvents();
            }
        }
    }
}