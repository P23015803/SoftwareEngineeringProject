using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace Group_Project_SIMS
{
    public partial class ManageCalendar : System.Web.UI.Page
    {
        SqlConnection con =
        new SqlConnection(
        ConfigurationManager.ConnectionStrings["SIMS"]
        .ConnectionString);

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
                LoadData();
            }
        }

        private void LoadData(string title = null, string type = null, string semester = null)
        {
            // Build parameterized query with optional filters
            string sql = "SELECT * FROM AcademicCalendar WHERE 1=1";
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

            // Order by CalendarID to ensure stable, predictable ordering
            sql += " ORDER BY CalendarID ASC";

            cmd.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            // attempt to resolve CreatedBy to user's FullName
            try
            {
                DataTable users = new DataTable();
                SqlDataAdapter uda = new SqlDataAdapter("SELECT UserID, FullName FROM Users", con);
                uda.Fill(users);

                if (!dt.Columns.Contains("CreatedByName"))
                {
                    dt.Columns.Add("CreatedByName", typeof(string));
                }

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
                // ignore mapping errors and fall back to raw CreatedBy value
                if (!dt.Columns.Contains("CreatedByName"))
                {
                    dt.Columns.Add("CreatedByName", typeof(string));
                    foreach (DataRow r in dt.Rows)
                        r["CreatedByName"] = r["CreatedBy"] == DBNull.Value ? string.Empty : r["CreatedBy"].ToString();
                }
            }

            gvCalendar.DataSource = dt;
            gvCalendar.DataBind();
        }

        protected void btnSave_Click(
            object sender,
            EventArgs e)
        {
            // determine creator id (try Claims/Identity then Session fallback)
            string createdBy = null;
            if (User != null && User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
            {
                createdBy = User.Identity.Name;
            }
            else if (Session != null && Session["UserID"] != null)
            {
                createdBy = Session["UserID"].ToString();
            }
            else
            {
                createdBy = "System"; // fallback value when no user context is available
            }

            SqlCommand cmd =
            new SqlCommand(
            @"INSERT INTO AcademicCalendar
            (
                EventTitle,
                EventDescription,
                EventDate,
                EventType,
                Semester,
                Status,
                PublishStatus,
                CreatedBy,
                CreatedDate
            )
            VALUES
            (
                @Title,
                @Description,
                @Date,
                @Type,
                @Semester,
                'Approved',
                'Published',
                @CreatedBy,
                @CreatedDate
            )", con);

            cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
            cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
            cmd.Parameters.AddWithValue("@Date", txtDate.Text);
            cmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
            cmd.Parameters.AddWithValue("@Semester", ddlSemester.SelectedValue);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // reload using current filters (treat first dropdown item as 'no filter')
            string typeFilter = (ddlFilterType != null && ddlFilterType.SelectedIndex > 0) ? ddlFilterType.SelectedValue : null;
            string semesterFilter = (ddlFilterSemester != null && ddlFilterSemester.SelectedIndex > 0) ? ddlFilterSemester.SelectedValue : null;
            LoadData(txtSearchTitle.Text, typeFilter, semesterFilter);
            ClearFields();
        }

        protected void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            SqlCommand cmd =
            new SqlCommand(
            @"UPDATE AcademicCalendar
            SET
            EventTitle=@Title,
            EventDescription=@Description,
            EventDate=@Date,
            EventType=@Type,
            Semester=@Semester
            WHERE CalendarID=@ID", con);

            cmd.Parameters.AddWithValue(
            "@ID",
            hfCalendarID.Value);

            cmd.Parameters.AddWithValue(
            "@Title",
            txtTitle.Text);

            cmd.Parameters.AddWithValue(
            "@Description",
            txtDescription.Text);

            cmd.Parameters.AddWithValue(
            "@Date",
            txtDate.Text);

            cmd.Parameters.AddWithValue(
            "@Type",
            ddlType.SelectedValue);

            cmd.Parameters.AddWithValue(
            "@Semester",
            ddlSemester.SelectedValue);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // reload using current filters (treat first dropdown item as 'no filter')
            string typeFilter = (ddlFilterType != null && ddlFilterType.SelectedIndex > 0) ? ddlFilterType.SelectedValue : null;
            string semesterFilter = (ddlFilterSemester != null && ddlFilterSemester.SelectedIndex > 0) ? ddlFilterSemester.SelectedValue : null;
            LoadData(txtSearchTitle.Text, typeFilter, semesterFilter);
            ClearFields();
        }

        private void ClearFields()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtDate.Text = "";
        }

        protected void gvCalendar_RowCommand(
            object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int index =
            Convert.ToInt32(e.CommandArgument);

            GridViewRow row =
            gvCalendar.Rows[index];

            string id =
            row.Cells[0].Text;

            if (e.CommandName == "SelectRecord")
            {
                hfCalendarID.Value = id;

                txtTitle.Text =
                row.Cells[1].Text;
                // populate description
                txtDescription.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);

                // date is in the EventDate column (cell index 4)
                var lbl = row.Cells[4].FindControl("lblDate") as Label;
                string raw = lbl != null ? lbl.Text : HttpUtility.HtmlDecode(row.Cells[4].Text);
                raw = (raw ?? "").Trim();

                // handle empty or non-date
                if (string.IsNullOrEmpty(raw) || raw == "&nbsp;")
                {
                    txtDate.Text = ""; // or set a default / show message
                }
                else
                {
                    DateTime parsed;
                    // try general parse first (uses current culture)
                    if (DateTime.TryParse(raw, out parsed) ||
                        // try common explicit formats if needed
                        DateTime.TryParseExact(raw, new[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" },
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                    {
                        txtDate.Text = parsed.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        // handle parse failure (log, user message, or leave blank)
                        txtDate.Text = ""; // or show error
                    }
                }

                ddlType.SelectedValue =
                row.Cells[5].Text;

                ddlSemester.SelectedValue =
                row.Cells[6].Text;
            }

            if (e.CommandName == "DeleteRecord")
            {
                SqlCommand cmd =
                new SqlCommand(
                "DELETE FROM AcademicCalendar WHERE CalendarID=@ID",
                con);

                cmd.Parameters.AddWithValue(
                "@ID",
                id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                // reload using current filters (treat first dropdown item as 'no filter')
                string typeFilterLocal = (ddlFilterType != null && ddlFilterType.SelectedIndex > 0) ? ddlFilterType.SelectedValue : null;
                string semesterFilterLocal = (ddlFilterSemester != null && ddlFilterSemester.SelectedIndex > 0) ? ddlFilterSemester.SelectedValue : null;
                LoadData(txtSearchTitle.Text, typeFilterLocal, semesterFilterLocal);
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string tFilter = (ddlFilterType != null && ddlFilterType.SelectedIndex > 0) ? ddlFilterType.SelectedValue : null;
            string sFilter = (ddlFilterSemester != null && ddlFilterSemester.SelectedIndex > 0) ? ddlFilterSemester.SelectedValue : null;
            LoadData(txtSearchTitle.Text, tFilter, sFilter);
        }

        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtSearchTitle.Text = string.Empty;
            if (ddlFilterType.Items.Count > 0) ddlFilterType.SelectedIndex = 0;
            if (ddlFilterSemester.Items.Count > 0) ddlFilterSemester.SelectedIndex = 0;
            LoadData();
        }
    }
}