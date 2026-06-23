using Group_Project_SIMS;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Group_Project_SIMS
{
    public partial class Programme : System.Web.UI.Page
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
                LoadLecturers();
                LoadProgramme();
            }
        }

        // ADD PROGRAMME
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == "" || txtCode.Text.Trim() == "" || txtDuration.Text.Trim() == "")
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Please fill all fields!";
                    return;
                }

                int durationYears;
                if (!int.TryParse(txtDuration.Text.Trim(), out durationYears) || durationYears < 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Please enter a valid non-negative number for Duration (Years).";
                    return;
                }

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    int lecturerId = 0;
                    if (!int.TryParse(ddlLecturer.SelectedValue, out lecturerId) || lecturerId <= 0)
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "Please select Head of Programme.";
                        return;
                    }

                    string query = "INSERT INTO Programmes (ProgrammeName, ProgrammeCode, DurationYears, LecturerID) VALUES (@name, @code, @duration, @lecturer)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@code", txtCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@duration", durationYears);
                    cmd.Parameters.AddWithValue("@lecturer", lecturerId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Programme added successfully!";

                txtName.Text = "";
                txtCode.Text = "";
                txtDuration.Text = "";
                ddlLecturer.ClearSelection();

                LoadProgramme();
            }
            catch (Exception ex)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Error: " + ex.Message;
            }
        }

        // LOAD DATA
        void LoadProgramme(string search = null)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT p.ProgrammeID, p.ProgrammeName, p.ProgrammeCode, p.DurationYears, p.LecturerID, u.FullName AS LecturerName FROM Programmes p LEFT JOIN Lecturers l ON p.LecturerID = l.LecturerID LEFT JOIN Users u ON l.UserID = u.UserID";

                var whereClauses = new System.Collections.Generic.List<string>();
                var cmd = new SqlCommand();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClauses.Add("(p.ProgrammeName LIKE @search OR p.ProgrammeCode LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", "%" + search.Trim() + "%");
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

                gvProgramme.DataSource = dt;
                gvProgramme.DataBind();
            }
        }

        // LOAD LECTURERS FOR DROPDOWNS
        void LoadLecturers()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                // Lecturers table stores UserID as FK; get only users with RoleID = 2 (lecturers)
                // Only include lecturers who have HOPprivileges = 1
                SqlDataAdapter da = new SqlDataAdapter("SELECT l.LecturerID, u.FullName AS LecturerName FROM Lecturers l INNER JOIN Users u ON l.UserID = u.UserID WHERE u.RoleID = 2 AND ISNULL(l.HOPprivileges,0) = 1", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlLecturer.DataSource = dt;
                ddlLecturer.DataValueField = "LecturerID";
                ddlLecturer.DataTextField = "LecturerName";
                ddlLecturer.DataBind();

                ddlLecturer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Head of Programme --", "0"));
            }
        }

        // Populate edit dropdown inside GridView when in edit mode
        protected void gvProgramme_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow && (e.Row.RowState & System.Web.UI.WebControls.DataControlRowState.Edit) == System.Web.UI.WebControls.DataControlRowState.Edit)
            {
                var ddl = e.Row.FindControl("ddlEditLecturer") as System.Web.UI.WebControls.DropDownList;
                if (ddl != null)
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        // Populate edit dropdown with LecturerID and Users.FullName for RoleID = 2
                        // Only include lecturers who have HOPprivileges = 1
                        SqlDataAdapter da = new SqlDataAdapter("SELECT l.LecturerID, u.FullName AS LecturerName FROM Lecturers l INNER JOIN Users u ON l.UserID = u.UserID WHERE u.RoleID = 2 AND ISNULL(l.HOPprivileges,0) = 1", con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddl.DataSource = dt;
                        ddl.DataValueField = "LecturerID";
                        ddl.DataTextField = "LecturerName";
                        ddl.DataBind();

                        ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Head of Programme --", "0"));

                        // Select current value if it exists in the filtered list; otherwise default to the placeholder
                        var dataItem = e.Row.DataItem as DataRowView;
                        if (dataItem != null && dataItem["LecturerID"] != DBNull.Value)
                        {
                            string current = dataItem["LecturerID"].ToString();
                            var found = ddl.Items.FindByValue(current);
                            if (found != null)
                            {
                                ddl.SelectedValue = current;
                            }
                            else
                            {
                                ddl.SelectedIndex = 0; // current lecturer is not a valid HOP anymore
                            }
                        }
                    }
                }
            }
        }

        // EDIT MODE
        protected void gvProgramme_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvProgramme.EditIndex = e.NewEditIndex;
            LoadProgramme();
        }

        // CANCEL
        protected void gvProgramme_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvProgramme.EditIndex = -1;
            LoadProgramme();
        }

        // UPDATE
        protected void gvProgramme_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvProgramme.DataKeys[e.RowIndex].Values["ProgrammeID"]);

            var row = gvProgramme.Rows[e.RowIndex];
            var txtNameEdit = row.FindControl("txtEditName") as System.Web.UI.WebControls.TextBox;
            var txtCodeEdit = row.FindControl("txtEditCode") as System.Web.UI.WebControls.TextBox;
            var txtDurationEdit = row.FindControl("txtEditDuration") as System.Web.UI.WebControls.TextBox;
            var ddlEditLecturer = row.FindControl("ddlEditLecturer") as System.Web.UI.WebControls.DropDownList;

            string name = txtNameEdit?.Text ?? string.Empty;
            string code = txtCodeEdit?.Text ?? string.Empty;
            string durationText = txtDurationEdit?.Text ?? string.Empty;

            int durationYears = 0;
            if (!int.TryParse(durationText, out durationYears) || durationYears < 0)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please enter a valid non-negative number for Duration (Years).";
                return;
            }

            int lecturerId = 0;
            if (ddlEditLecturer != null)
            {
                int.TryParse(ddlEditLecturer.SelectedValue, out lecturerId);
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "UPDATE Programmes SET ProgrammeName=@name, ProgrammeCode=@code, DurationYears=@duration, LecturerID=@lecturer WHERE ProgrammeID=@id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@duration", durationYears);
                cmd.Parameters.AddWithValue("@lecturer", lecturerId > 0 ? (object)lecturerId : DBNull.Value);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvProgramme.EditIndex = -1;
            LoadProgramme();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Programme updated successfully!";
        }

        // DELETE
        protected void gvProgramme_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvProgramme.DataKeys[e.RowIndex].Values["ProgrammeID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Programmes WHERE ProgrammeID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadProgramme();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Programme deleted successfully!";
        }

        // SEARCH BUTTON
        protected void btnSearchProgramme_Click(object sender, EventArgs e)
        {
            string search = txtSearchProgramme.Text.Trim();
            LoadProgramme(search);
        }

        // RESET SEARCH
        protected void btnResetProgramme_Click(object sender, EventArgs e)
        {
            txtSearchProgramme.Text = string.Empty;
            LoadProgramme();
        }
    }
}