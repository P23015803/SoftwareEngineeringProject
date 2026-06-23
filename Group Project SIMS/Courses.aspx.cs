using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Group_Project_SIMS
{
    public partial class Course : System.Web.UI.Page
    {
        string connStr = @"Data Source=LAPTOP-0BUOURQ9\SQLEXPRESS;Initial Catalog=SIMS;Integrated Security=True";

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
                LoadProgramme();
                LoadLecturers();
                LoadFilterProgrammes();
                LoadCourse();
            }
        }

        // LOAD PROGRAMME DROPDOWN
        void LoadProgramme()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT ProgrammeID, ProgrammeName FROM Programmes", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlProgramme.DataSource = dt;
                ddlProgramme.DataTextField = "ProgrammeName";
                ddlProgramme.DataValueField = "ProgrammeID";
                ddlProgramme.DataBind();

                ddlProgramme.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Programme --", "0"));
            }
        }

        // LOAD LECTURERS FOR DROPDOWN
        void LoadLecturers()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT l.LecturerID, u.FullName AS LecturerName FROM Lecturers l INNER JOIN Users u ON l.UserID = u.UserID WHERE u.RoleID = 2", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlLecturer.DataSource = dt;
                ddlLecturer.DataValueField = "LecturerID";
                ddlLecturer.DataTextField = "LecturerName";
                ddlLecturer.DataBind();

                ddlLecturer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Lecturer --", "0"));
            }
        }

        // ADD COURSE
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == "" || txtCode.Text.Trim() == "" || txtCredit.Text.Trim() == "")
                {
                    lblMsg.Text = "Fill all fields!";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                int creditHours;
                if (!int.TryParse(txtCredit.Text.Trim(), out creditHours) || creditHours < 0)
                {
                    lblMsg.Text = "Please enter a valid non-negative number for Credit Hours.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                int programmeId = 0;
                if (!int.TryParse(ddlProgramme.SelectedValue, out programmeId) || programmeId <= 0)
                {
                    lblMsg.Text = "Please select a programme.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                int lecturerId = 0;
                if (!int.TryParse(ddlLecturer.SelectedValue, out lecturerId) || lecturerId <= 0)
                {
                    lblMsg.Text = "Please select an assigned lecturer.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = "INSERT INTO Courses (CourseName, CourseCode, CreditHours, ProgrammeID, LecturerID) VALUES (@name,@code,@credit,@pid,@lid)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@code", txtCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@credit", creditHours);
                    cmd.Parameters.AddWithValue("@pid", programmeId);
                    cmd.Parameters.AddWithValue("@lid", lecturerId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMsg.Text = "Course added successfully!";
                lblMsg.ForeColor = System.Drawing.Color.Green;

                txtName.Text = "";
                txtCode.Text = "";
                txtCredit.Text = "";
                ddlProgramme.ClearSelection();
                ddlLecturer.ClearSelection();

                LoadCourse();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        // LOAD COURSE LIST (supports optional search by name/code and filter by programme)
        void LoadCourse(string search = null, int? programmeId = null)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT c.CourseID, c.CourseName, c.CourseCode, c.CreditHours, c.ProgrammeID, c.LecturerID,
                                 p.ProgrammeName, u.FullName AS LecturerName
                                 FROM Courses c
                                 LEFT JOIN Programmes p ON c.ProgrammeID = p.ProgrammeID
                                 LEFT JOIN Lecturers l ON c.LecturerID = l.LecturerID
                                 LEFT JOIN Users u ON l.UserID = u.UserID";

                // build WHERE clauses based on parameters
                var whereClauses = new System.Collections.Generic.List<string>();
                var cmd = new SqlCommand();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClauses.Add("(c.CourseName LIKE @search OR c.CourseCode LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", "%" + search.Trim() + "%");
                }

                if (programmeId.HasValue && programmeId.Value > 0)
                {
                    whereClauses.Add("c.ProgrammeID = @pid");
                    cmd.Parameters.AddWithValue("@pid", programmeId.Value);
                }

                if (whereClauses.Count > 0)
                {
                    query += " WHERE " + string.Join(" AND ", whereClauses);
                }

                cmd.CommandText = query;
                cmd.Connection = con;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCourse.DataSource = dt;
                gvCourse.DataBind();
            }
        }

        // Populate programme dropdown used to filter courses
        void LoadFilterProgrammes()
        {
            // ddlFilterProgramme is defined in the .aspx markup
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT ProgrammeID, ProgrammeName FROM Programmes", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlFilterProgramme.DataSource = dt;
                ddlFilterProgramme.DataTextField = "ProgrammeName";
                ddlFilterProgramme.DataValueField = "ProgrammeID";
                ddlFilterProgramme.DataBind();

                ddlFilterProgramme.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All Programmes --", "0"));
            }
        }

        // Populate edit dropdowns inside GridView when in edit mode
        protected void gvCourse_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow && (e.Row.RowState & System.Web.UI.WebControls.DataControlRowState.Edit) == System.Web.UI.WebControls.DataControlRowState.Edit)
            {
                var ddlProg = e.Row.FindControl("ddlEditProgramme") as System.Web.UI.WebControls.DropDownList;
                var ddlLect = e.Row.FindControl("ddlEditLecturer") as System.Web.UI.WebControls.DropDownList;

                if (ddlProg != null)
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT ProgrammeID, ProgrammeName FROM Programmes", con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlProg.DataSource = dt;
                        ddlProg.DataValueField = "ProgrammeID";
                        ddlProg.DataTextField = "ProgrammeName";
                        ddlProg.DataBind();

                        ddlProg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Programme --", "0"));

                        var dataItem = e.Row.DataItem as DataRowView;
                        if (dataItem != null && dataItem["ProgrammeID"] != DBNull.Value)
                        {
                            ddlProg.SelectedValue = dataItem["ProgrammeID"].ToString();
                        }
                    }
                }

                if (ddlLect != null)
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT l.LecturerID, u.FullName AS LecturerName FROM Lecturers l INNER JOIN Users u ON l.UserID = u.UserID WHERE u.RoleID = 2", con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlLect.DataSource = dt;
                        ddlLect.DataValueField = "LecturerID";
                        ddlLect.DataTextField = "LecturerName";
                        ddlLect.DataBind();

                        ddlLect.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Lecturer --", "0"));

                        var dataItem = e.Row.DataItem as DataRowView;
                        if (dataItem != null && dataItem["LecturerID"] != DBNull.Value)
                        {
                            ddlLect.SelectedValue = dataItem["LecturerID"].ToString();
                        }
                    }
                }
            }
        }

        // EDIT
        protected void gvCourse_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvCourse.EditIndex = e.NewEditIndex;
            LoadCourse();
        }

        // CANCEL
        protected void gvCourse_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvCourse.EditIndex = -1;
            LoadCourse();
        }

        // UPDATE
        protected void gvCourse_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvCourse.DataKeys[e.RowIndex].Values["CourseID"]);

            var row = gvCourse.Rows[e.RowIndex];
            var txtNameEdit = row.FindControl("txtEditName") as System.Web.UI.WebControls.TextBox;
            var txtCodeEdit = row.FindControl("txtEditCode") as System.Web.UI.WebControls.TextBox;
            var txtCreditEdit = row.FindControl("txtEditCredit") as System.Web.UI.WebControls.TextBox;
            var ddlEditProgramme = row.FindControl("ddlEditProgramme") as System.Web.UI.WebControls.DropDownList;
            var ddlEditLecturer = row.FindControl("ddlEditLecturer") as System.Web.UI.WebControls.DropDownList;

            string name = txtNameEdit?.Text ?? string.Empty;
            string code = txtCodeEdit?.Text ?? string.Empty;
            string creditText = txtCreditEdit?.Text ?? string.Empty;

            int creditHours = 0;
            if (!int.TryParse(creditText, out creditHours) || creditHours < 0)
            {
                lblMsg.Text = "Please enter a valid non-negative number for Credit Hours.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int programmeId = 0;
            if (ddlEditProgramme != null)
            {
                int.TryParse(ddlEditProgramme.SelectedValue, out programmeId);
            }

            int lecturerId = 0;
            if (ddlEditLecturer != null)
            {
                int.TryParse(ddlEditLecturer.SelectedValue, out lecturerId);
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "UPDATE Courses SET CourseName=@name, CourseCode=@code, CreditHours=@credit, ProgrammeID=@pid, LecturerID=@lid WHERE CourseID=@id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@credit", creditHours);
                cmd.Parameters.AddWithValue("@pid", programmeId > 0 ? (object)programmeId : DBNull.Value);
                cmd.Parameters.AddWithValue("@lid", lecturerId > 0 ? (object)lecturerId : DBNull.Value);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvCourse.EditIndex = -1;
            LoadCourse();

            lblMsg.Text = "Course updated successfully!";
            lblMsg.ForeColor = System.Drawing.Color.Green;
        }

        // DELETE
        protected void gvCourse_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvCourse.DataKeys[e.RowIndex].Values["CourseID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Courses WHERE CourseID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadCourse();

            lblMsg.Text = "Course deleted successfully!";
            lblMsg.ForeColor = System.Drawing.Color.Green;
        }

        // SEARCH BUTTON
        protected void btnSearchCourse_Click(object sender, EventArgs e)
        {
            string search = txtSearchCourse.Text.Trim();
            int selectedPid = 0;
            int.TryParse(ddlFilterProgramme.SelectedValue, out selectedPid);
            LoadCourse(search, selectedPid > 0 ? (int?)selectedPid : null);
        }

        // RESET SEARCH
        protected void btnResetCourse_Click(object sender, EventArgs e)
        {
            txtSearchCourse.Text = string.Empty;
            ddlFilterProgramme.ClearSelection();
            var item = ddlFilterProgramme.Items.FindByValue("0");
            if (item != null) item.Selected = true;
            LoadCourse();
        }

    }
}
 