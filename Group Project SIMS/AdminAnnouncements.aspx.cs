using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SoftwareEngineeringProject
{
    public partial class AdminAnnouncements : System.Web.UI.Page
    {
        // Centralized connection string matching your other modules
        string connectionString = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Check: Ensure an Admin is logged in
            if (Session["UserID"] == null)
            {
                Response.Redirect("AdminLogin.aspx"); // Update this to match your actual login page name
                return;
            }

            if (!IsPostBack)
            {
                LoadAnnouncements();
            }
        }

        // --- 1. VIEW ANNOUNCEMENTS ---
        private void LoadAnnouncements()
        {
            // Updated column names: Content instead of MessageBody, CreatedDate instead of DatePosted
            string query = @"
                SELECT Title, Content, AuthorName, CreatedDate 
                FROM Announcements 
                WHERE AuthorRole = 'Admin' 
                ORDER BY CreatedDate DESC;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        try
                        {
                            conn.Open();
                            sda.Fill(dt);
                            gvAnnouncements.DataSource = dt;
                            gvAnnouncements.DataBind();
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Error loading feed: " + ex.Message;
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }

        // --- 2. POST ANNOUNCEMENT ---
        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtContent.Text))
            {
                lblMessage.Text = "Title and Content cannot be empty!";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Updated INSERT query mapping the exact table schema columns
            string query = @"
                INSERT INTO Announcements (Title, Content, AuthorRole, AuthorName, CourseID, CreatedDate, CreatedBy) 
                VALUES (@Title, @Content, @Role, @Name, @Course, GETDATE(), @CreatedBy);";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Content", txtContent.Text.Trim());
                    cmd.Parameters.AddWithValue("@Role", "Admin");

                    // Safely pull the active logged-in user's name if stored in session, otherwise fallback
                    string authorName = Session["FullName"] != null ? Session["FullName"].ToString() : "System Admin";
                    cmd.Parameters.AddWithValue("@Name", authorName);

                    cmd.Parameters.AddWithValue("@Course", DBNull.Value); // Global admin post
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(Session["UserID"])); // Links to Users table

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        txtTitle.Text = "";
                        txtContent.Text = "";
                        lblMessage.Text = "Announcement posted successfully!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;

                        LoadAnnouncements(); // Refresh the grid
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error posting: " + ex.Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
    }
}