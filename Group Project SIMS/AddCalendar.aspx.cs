using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Group_Project_SIMS
{
    public partial class AddCalendar : System.Web.UI.Page
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["SIMS"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Fix validation issue
            UnobtrusiveValidationMode =
                UnobtrusiveValidationMode.None;

            // Check login
            if (Session["UserID"] == null)
            {
                Response.Redirect("Admin Login.aspx");
                return;
            }

            // Role verification: allow RoleID 1 (Admin) or 2 (Lecturer)
            if (Session["RoleID"] == null)
            {
                Response.Redirect("Admin Login.aspx");
                return;
            }

            int roleId;
            if (!int.TryParse(Session["RoleID"].ToString(), out roleId) || (roleId != 1 && roleId != 2))
            {
                Response.Redirect("Admin Login.aspx");
                return;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;
                    lblMessage.Text =
                        "Please enter Event Title.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDate.Text))
                {
                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;
                    lblMessage.Text =
                        "Please enter Event Date.";
                    return;
                }

                DateTime eventDate;

                if (!DateTime.TryParse(
                    txtDate.Text,
                    out eventDate))
                {
                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;
                    lblMessage.Text =
                        "Invalid Event Date.";
                    return;
                }

                int createdBy =
                    Convert.ToInt32(Session["UserID"]);

                string query =
                @"INSERT INTO AcademicCalendar
                (
                    EventTitle,
                    EventDescription,
                    EventDate,
                    EventType,
                    Semester,
                    CreatedBy,
                    Status,
                    PublishStatus
                )
                VALUES
                (
                    @EventTitle,
                    @EventDescription,
                    @EventDate,
                    @EventType,
                    @Semester,
                    @CreatedBy,
                    @Status,
                    @PublishStatus
                )";

                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@EventTitle",
                            txtTitle.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@EventDescription",
                            txtDescription.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@EventDate",
                            eventDate);

                        cmd.Parameters.AddWithValue(
                            "@EventType",
                            ddlType.SelectedValue);

                        cmd.Parameters.AddWithValue(
                            "@Semester",
                            ddlSemester.SelectedValue);

                        cmd.Parameters.AddWithValue(
                            "@CreatedBy",
                            createdBy);

                        cmd.Parameters.AddWithValue(
                            "@Status",
                            "Pending");

                        cmd.Parameters.AddWithValue(
                            "@PublishStatus",
                            "Unpublished");

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                lblMessage.ForeColor =
                    System.Drawing.Color.Green;

                lblMessage.Text =
                    "Calendar event added successfully.";

                ClearFields();
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor =
                    System.Drawing.Color.Red;

                lblMessage.Text =
                    "Error: " + ex.Message;
            }
        }

        protected void btnClear_Click(
            object sender,
            EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtDate.Text = "";

            if (ddlType.Items.Count > 0)
                ddlType.SelectedIndex = 0;

            if (ddlSemester.Items.Count > 0)
                ddlSemester.SelectedIndex = 0;
        }
    }
}