using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Group_Project_SIMS
{
    public partial class RegisterUsers
        : System.Web.UI.Page
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
                LoadProgrammes();
                // populate search programmes dropdown from loaded programmes
                if (ddlSearchProgramme != null && ddlProgramme != null)
                {
                    ddlSearchProgramme.Items.Clear();
                    ddlSearchProgramme.Items.Add(new System.Web.UI.WebControls.ListItem("All Programmes", ""));
                    foreach (System.Web.UI.WebControls.ListItem it in ddlProgramme.Items)
                    {
                        // avoid duplicating default empty items
                        if (!string.IsNullOrWhiteSpace(it.Value))
                            ddlSearchProgramme.Items.Add(new System.Web.UI.WebControls.ListItem(it.Text, it.Value));
                    }
                }

                // populate admission status and HOP filters defaults
                if (ddlSearchAdmissionStatus != null)
                {
                    ddlSearchAdmissionStatus.Items.Clear();
                    ddlSearchAdmissionStatus.Items.Add(new System.Web.UI.WebControls.ListItem("All Statuses", ""));
                    ddlSearchAdmissionStatus.Items.Add(new System.Web.UI.WebControls.ListItem("Pending", "Pending"));
                    ddlSearchAdmissionStatus.Items.Add(new System.Web.UI.WebControls.ListItem("Accepted", "Accepted"));
                }

                if (ddlSearchHOP != null)
                {
                    ddlSearchHOP.Items.Clear();
                    ddlSearchHOP.Items.Add(new System.Web.UI.WebControls.ListItem("All Lecturers", ""));
                    ddlSearchHOP.Items.Add(new System.Web.UI.WebControls.ListItem("HOPs", "1"));
                    ddlSearchHOP.Items.Add(new System.Web.UI.WebControls.ListItem("Normal Lecturers", "0"));
                }

                // default sort direction
                ViewState["SortDirection"] = "ASC";

                LoadUsers(rblSearchRole.SelectedValue);
            }
        }
        private void LoadProgrammes() //Displays programmes
        {
            using (SqlConnection conn =
                  new SqlConnection(
                      connectionString))
            {
                conn.Open();

                string query =
                @"SELECT
            ProgrammeID,
            ProgrammeName
          FROM Programmes";

                SqlCommand cmd =
                    new SqlCommand(
                        query,
                        conn);

                SqlDataReader reader =
                    cmd.ExecuteReader();

                ddlProgramme.DataSource = reader;

                ddlProgramme.DataTextField =
                    "ProgrammeName";

                ddlProgramme.DataValueField =
                    "ProgrammeID";

                ddlProgramme.DataBind();
            }
        }

        // Show/hide additional registration fields based on role selection
        protected void rblRole_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            pnlLecturer.Visible = false;
            pnlStudent.Visible = false;

            if (rblRole.SelectedValue == "2")
            {
                pnlLecturer.Visible = true;
            }

            else if (rblRole.SelectedValue == "3")
            {
                pnlStudent.Visible = true;
            }
        }


        //show/hide gridview tables based on search role selection
        protected void rblSearchRole_SelectedIndexChanged(object sender, EventArgs e)
        { 
            LoadUsers(rblSearchRole.SelectedValue);
            pnlStudentGrid.Visible = false;
            pnlLecturerGrid.Visible = false;

            if (rblSearchRole.SelectedValue == "Lecturer")
            {
                pnlLecturerGrid.Visible = true;
            }

            else if (rblSearchRole.SelectedValue == "Student")
            {
                pnlStudentGrid.Visible = true;
            }
        }



        // Displays users
        private void LoadUsers(string role)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "";
                if (role == "Student")
                {
                    query = @"SELECT U.UserID, S.StudentID, S.ProgrammeID, U.FullName, U.Email, P.ProgrammeName, S.IntakeYear, S.AdmissionStatus
                              FROM Users U INNER JOIN Students S ON U.UserID=S.UserID
                              INNER JOIN Programmes P ON S.ProgrammeID=P.ProgrammeID";
                }
                else if (role == "Lecturer")
                {
                    query = @"SELECT U.UserID, L.LecturerID, U.FullName, U.Email, L.Department, L.Specialization, L.HireDate, L.HOPprivileges
                              FROM Users U INNER JOIN Lecturers L ON U.UserID=L.UserID";
                }

                SqlDataAdapter adapter = new SqlDataAdapter(new SqlCommand(query, conn));
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (role == "Student")
                {
                    gvStudents.DataSource = dt;
                    gvStudents.DataBind();
                }
                else if (role == "Lecturer")
                {
                    gvLecturers.DataSource = dt;
                    gvLecturers.DataBind();
                }
            }
        }

        // STUDENT GRIDVIEW EVENTS
        protected void gvStudents_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            LoadUsers("Student");
        }

        protected void gvStudents_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            LoadUsers("Student");
        }

        protected void gvStudents_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow && e.Row.RowIndex == gvStudents.EditIndex)
            {
                // populate programmes dropdown in edit mode
                var ddl = e.Row.FindControl("ddlGridProgramme") as System.Web.UI.WebControls.DropDownList;
                if (ddl != null)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string q = "SELECT ProgrammeID, ProgrammeName FROM Programmes";
                        using (SqlCommand cmd = new SqlCommand(q, conn))
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            ddl.DataSource = rdr;
                            ddl.DataTextField = "ProgrammeName";
                            ddl.DataValueField = "ProgrammeID";
                            ddl.DataBind();
                        }
                    }

                    // set selected value from DataKeys
                    object val = gvStudents.DataKeys[e.Row.RowIndex].Values["ProgrammeID"];
                    if (val != null)
                    {
                        ddl.SelectedValue = val.ToString();
                    }
                }

                // Admission Status DropDown population
                var ddlStatus = e.Row.FindControl("ddlAdmissionStatusGrid") as DropDownList;
                var dataItem = e.Row.DataItem as DataRowView;
                if (ddlStatus != null && dataItem != null)
                {
                    string status = (dataItem["AdmissionStatus"] ?? string.Empty).ToString().Trim();
                    var item = ddlStatus.Items.FindByValue(status);
                    if (item != null) ddlStatus.SelectedValue = item.Value;
                    else ddlStatus.SelectedIndex = 0; // default to Pending
                }
            }
        }

        //Turn the edited values into variables and update the database with the new values
        protected void gvStudents_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int userID = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Values["UserID"]);

            var row = gvStudents.Rows[e.RowIndex];
            var txtFullName = row.FindControl("txtFullName") as System.Web.UI.WebControls.TextBox;
            var txtEmailGrid = row.FindControl("txtEmail") as System.Web.UI.WebControls.TextBox;
            var ddlGridProgramme = row.FindControl("ddlGridProgramme") as System.Web.UI.WebControls.DropDownList;
            var txtIntakeYearGrid = row.FindControl("txtIntakeYearGrid") as System.Web.UI.WebControls.TextBox;
            var ddlAdmissionStatusGrid = row.FindControl("ddlAdmissionStatusGrid") as System.Web.UI.WebControls.DropDownList;

            string fullName = txtFullName?.Text.Trim() ?? string.Empty;
            string email = txtEmailGrid?.Text.Trim() ?? string.Empty;
            int programmeID = ddlGridProgramme != null ? Convert.ToInt32(ddlGridProgramme.SelectedValue) : 0;
            int intakeYear = 0;
            int.TryParse(txtIntakeYearGrid?.Text.Trim(), out intakeYear);
            string admissionStatus = ddlAdmissionStatusGrid != null ? (ddlAdmissionStatusGrid.SelectedValue ?? string.Empty).Trim() : string.Empty;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    string updateUser = "UPDATE Users SET FullName=@FullName, Email=@Email WHERE UserID=@UserID";
                    using (SqlCommand cmd = new SqlCommand(updateUser, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }

                    string updateStudent = "UPDATE Students SET ProgrammeID=@ProgrammeID, IntakeYear=@IntakeYear, AdmissionStatus=@AdmissionStatus WHERE UserID=@UserID";
                    using (SqlCommand cmd = new SqlCommand(updateStudent, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ProgrammeID", programmeID);
                        cmd.Parameters.AddWithValue("@IntakeYear", intakeYear);
                        cmd.Parameters.AddWithValue("@AdmissionStatus", admissionStatus);
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    lblMessage.Text = "Student updated successfully.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblMessage.Text = ex.Message;
                }
            }

            gvStudents.EditIndex = -1;
            LoadUsers("Student");
        }

        protected void gvStudents_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int userID = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Values["UserID"]);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Students WHERE UserID=@UserID", conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE UserID=@UserID", conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    lblMessage.Text = "Student deleted.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblMessage.Text = ex.Message;
                }
            }

            LoadUsers("Student");
        }

        // LECTURER GRIDVIEW EVENTS
        protected void gvLecturers_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvLecturers.EditIndex = e.NewEditIndex;
            LoadUsers("Lecturer");
        }

        protected void ToggleSort_Click(object sender, EventArgs e)
        {
            string cur = (ViewState["SortDirection"] ?? "ASC").ToString();
            string next = cur == "ASC" ? "DESC" : "ASC";
            ViewState["SortDirection"] = next;

            // update button text to reflect next action
            if (btnToggleSort != null)
            {
                btnToggleSort.Text = next == "ASC" ? "Sort ↑" : "Sort ↓";
            }

            btnSearch_Click(sender, e);
        }

        protected void gvLecturers_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvLecturers.EditIndex = -1;
            LoadUsers("Lecturer");
        }

        protected void gvLecturers_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int userID = Convert.ToInt32(gvLecturers.DataKeys[e.RowIndex].Values["UserID"]);
            var row = gvLecturers.Rows[e.RowIndex];
            var txtName = row.FindControl("txtLectFullName") as System.Web.UI.WebControls.TextBox;
            var txtEmail = row.FindControl("txtLectEmail") as System.Web.UI.WebControls.TextBox;
            var txtDept = row.FindControl("txtDepartmentGrid") as System.Web.UI.WebControls.TextBox;
            var txtSpec = row.FindControl("txtSpecializationGrid") as System.Web.UI.WebControls.TextBox;
            var chkHOPGrid = row.FindControl("chkHOPGrid") as System.Web.UI.WebControls.CheckBox;

            string fullName = txtName?.Text.Trim() ?? string.Empty;
            string email = txtEmail?.Text.Trim() ?? string.Empty;
            string department = txtDept?.Text.Trim() ?? string.Empty;
            string specialization = txtSpec?.Text.Trim() ?? string.Empty;
            int hop = (chkHOPGrid != null && chkHOPGrid.Checked) ? 1 : 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    string updateUser = "UPDATE Users SET FullName=@FullName, Email=@Email WHERE UserID=@UserID";
                    using (SqlCommand cmd = new SqlCommand(updateUser, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }

                    string updateLect = "UPDATE Lecturers SET Department=@Department, Specialization=@Specialization, HOPprivileges=@HOP WHERE UserID=@UserID";
                    using (SqlCommand cmd = new SqlCommand(updateLect, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Department", department);
                        cmd.Parameters.AddWithValue("@Specialization", specialization);
                        cmd.Parameters.AddWithValue("@HOP", hop);
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    lblMessage.Text = "Lecturer updated successfully.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblMessage.Text = ex.Message;
                }
            }

            gvLecturers.EditIndex = -1;
            LoadUsers("Lecturer");
        }

        protected void gvLecturers_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int userID = Convert.ToInt32(gvLecturers.DataKeys[e.RowIndex].Values["UserID"]);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Lecturers WHERE UserID=@UserID", conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE UserID=@UserID", conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    lblMessage.Text = "Lecturer deleted.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblMessage.Text = ex.Message;
                }
            }

            LoadUsers("Lecturer");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string role = rblSearchRole.SelectedValue;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "";
                // no-op: preserve btnSearch_Click method during automated edits
                if (role == "Student")
                {
                    query = @"SELECT U.UserID, S.StudentID, S.ProgrammeID, U.FullName, U.Email, P.ProgrammeName, S.IntakeYear, S.AdmissionStatus
                              FROM Users U INNER JOIN Students S ON U.UserID=S.UserID
                              INNER JOIN Programmes P ON S.ProgrammeID=P.ProgrammeID
                              WHERE U.FullName LIKE @Search";

                    if (ddlSearchProgramme != null && !string.IsNullOrWhiteSpace(ddlSearchProgramme.SelectedValue))
                    {
                        query += " AND S.ProgrammeID = @ProgrammeID";
                    }

                    if (ddlSearchAdmissionStatus != null && !string.IsNullOrWhiteSpace(ddlSearchAdmissionStatus.SelectedValue))
                    {
                        query += " AND S.AdmissionStatus = @AdmissionStatus";
                    }
                }
                else // Lecturer
                {
                    query = @"SELECT U.UserID, L.LecturerID, U.FullName, U.Email, L.Department, L.Specialization, L.HireDate, L.HOPprivileges
                              FROM Users U INNER JOIN Lecturers L ON U.UserID=L.UserID
                              WHERE U.FullName LIKE @Search";

                    if (ddlSearchHOP != null && !string.IsNullOrWhiteSpace(ddlSearchHOP.SelectedValue))
                    {
                        // HOPprivileges stored as bit/int 1 or 0
                        query += " AND L.HOPprivileges = @HOPFlag";
                    }
                }
                // append ordering (numerical by UserID to handle gaps correctly)
                string sortDir = (ViewState["SortDirection"] ?? "ASC").ToString();
                query += " ORDER BY U.UserID " + (sortDir == "DESC" ? "DESC" : "ASC");

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Search", "%" + txtSearchDept.Text.Trim() + "%");

                if (role == "Student")
                {
                    if (ddlSearchProgramme != null && !string.IsNullOrWhiteSpace(ddlSearchProgramme.SelectedValue))
                        cmd.Parameters.AddWithValue("@ProgrammeID", Convert.ToInt32(ddlSearchProgramme.SelectedValue));

                    if (ddlSearchAdmissionStatus != null && !string.IsNullOrWhiteSpace(ddlSearchAdmissionStatus.SelectedValue))
                        cmd.Parameters.AddWithValue("@AdmissionStatus", ddlSearchAdmissionStatus.SelectedValue);
                }
                else
                {
                    if (ddlSearchHOP != null && !string.IsNullOrWhiteSpace(ddlSearchHOP.SelectedValue))
                        cmd.Parameters.AddWithValue("@HOPFlag", Convert.ToInt32(ddlSearchHOP.SelectedValue));
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (role == "Student")
                {
                    gvStudents.DataSource = dt;
                    gvStudents.DataBind();
                }
                else
                {
                    gvLecturers.DataSource = dt;
                    gvLecturers.DataBind();
                }
            }
        }

        protected void btnReset_Click(
            object sender,
            EventArgs e)
        {
            txtSearchDept.Text =
                string.Empty;

            LoadUsers(
                rblSearchRole.SelectedValue);
        }

        protected void btnRegister_Click(
            object sender,
            EventArgs e)
        {
            // clear previous message
            if (lblAllFields != null) lblAllFields.Visible = false;
            lblMessage.Text = string.Empty;

            // Server-side validation: ensure all required fields per role
            bool missing = false;
            if (string.IsNullOrWhiteSpace(txtName?.Text) || string.IsNullOrWhiteSpace(txtEmail?.Text) || string.IsNullOrWhiteSpace(txtPassword?.Text))
            {
                missing = true;
            }

            if (string.IsNullOrWhiteSpace(rblRole?.SelectedValue))
            {
                missing = true;
            }

            // role-specific checks
            if (!missing && rblRole.SelectedValue == "2") // Lecturer
            {
                if (string.IsNullOrWhiteSpace(txtDepartment?.Text) || string.IsNullOrWhiteSpace(txtSpecialization?.Text)) missing = true;
                // chkHOP is optional logically; do not enforce checked state
            }

            if (!missing && rblRole.SelectedValue == "3") // Student
            {
                if (ddlProgramme == null || string.IsNullOrWhiteSpace(ddlProgramme.SelectedValue)) missing = true;
                if (string.IsNullOrWhiteSpace(txtIntakeYear?.Text)) missing = true;
                if (ddlAdmissionStatus == null || string.IsNullOrWhiteSpace(ddlAdmissionStatus.SelectedValue)) missing = true;
            }

            if (missing)
            {
                if (lblAllFields != null)
                {
                    lblAllFields.Text = "All fields must be filled";
                    lblAllFields.Visible = true;
                }
                else
                {
                    lblMessage.Text = "All fields must be filled.";
                }

                return;
            }
            string fullName =
                txtName.Text.Trim();

            string email =
                txtEmail.Text.Trim();

            string password =
                txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(
                rblRole.SelectedValue))
            {
                lblMessage.Text =
                "Please select a role.";

                return;
            }

            int roleID =
            Convert.ToInt32(
                rblRole.SelectedValue);

            using (SqlConnection conn =
                  new SqlConnection(
                      connectionString))
            {
                conn.Open();

                SqlTransaction transaction =
                    conn.BeginTransaction();

                try
                {
                    //--------------------------------
                    // INSERT USERS
                    //--------------------------------

                    string userQuery =
                    @"
                    INSERT INTO Users
                    (
                        FullName,
                        Email,
                        PasswordHash,
                        RoleID
                    )

                    VALUES
                    (
                        @FullName,
                        @Email,
                        @Password,
                        @RoleID
                    );

                    SELECT SCOPE_IDENTITY();
                    ";

                    SqlCommand userCmd =
                        new SqlCommand(
                            userQuery,
                            conn,
                            transaction);

                    userCmd.Parameters
                        .AddWithValue(
                            "@FullName",
                            fullName);

                    userCmd.Parameters
                        .AddWithValue(
                            "@Email",
                            email);

                    userCmd.Parameters
                        .AddWithValue(
                            "@Password",
                            password);

                    userCmd.Parameters
                        .AddWithValue(
                            "@RoleID",
                            roleID);

                    int userID =
                    Convert.ToInt32(
                        userCmd
                        .ExecuteScalar());

                    //--------------------------------
                    // LECTURER INSERT
                    //--------------------------------

                    if (roleID == 2)
                    {
                        string department =
                            txtDepartment
                            .Text.Trim();

                        string specialization =
                            txtSpecialization
                            .Text.Trim();

                        int hop = (chkHOP != null && chkHOP.Checked) ? 1 : 0;

                        string lecturerQuery =
                        @"
                        INSERT INTO Lecturers
                        (
                            UserID,
                            Department,
                            Specialization
                            ,HOPprivileges
                        )

                        VALUES
                        (
                            @UserID,
                            @Department,
                            @Specialization
                            ,@HOP
                        )
                        ";

                        SqlCommand lecturerCmd =
                            new SqlCommand(
                                lecturerQuery,
                                conn,
                                transaction);

                        lecturerCmd.Parameters
                            .AddWithValue(
                                "@UserID",
                                userID);

                        lecturerCmd.Parameters
                            .AddWithValue(
                                "@Department",
                                department);

                        lecturerCmd.Parameters
                            .AddWithValue(
                                "@Specialization",
                                specialization);

                        lecturerCmd.Parameters
                            .AddWithValue(
                                "@HOP",
                                hop);

                        lecturerCmd
                            .ExecuteNonQuery();
                    }

                    //--------------------------------
                    // STUDENT INSERT
                    //--------------------------------

                    else if (roleID == 3)
                    {
                        int programmeID =
                        Convert.ToInt32(
                            ddlProgramme
                            .SelectedValue);

                        int intakeYear =
                        Convert.ToInt32(
                            txtIntakeYear.Text);

                        string admissionStatus =
                            (ddlAdmissionStatus != null ? ddlAdmissionStatus.SelectedValue : string.Empty).Trim();

                        string studentQuery =
                        @"
                        INSERT INTO Students
                        (
                            UserID,
                            ProgrammeID,
                            IntakeYear,
                            AdmissionStatus
                        )

                        VALUES
                        (
                            @UserID,
                            @ProgrammeID,
                            @IntakeYear,
                            @AdmissionStatus
                        )
                        ";

                        SqlCommand studentCmd =
                            new SqlCommand(
                                studentQuery,
                                conn,
                                transaction);

                        studentCmd.Parameters
                            .AddWithValue(
                                "@UserID",
                                userID);

                        studentCmd.Parameters
                            .AddWithValue(
                                "@ProgrammeID",
                                programmeID);

                        studentCmd.Parameters
                            .AddWithValue(
                                "@IntakeYear",
                                intakeYear);

                        studentCmd.Parameters
                            .AddWithValue(
                                "@AdmissionStatus",
                                admissionStatus);

                        studentCmd
                            .ExecuteNonQuery();
                    }

                    //--------------------------------
                    // SUCCESS
                    //--------------------------------

                    transaction.Commit();

                    lblMessage.Text =
                        "Registration successful!";

                    // Clear form fields
                    txtName.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    rblRole.ClearSelection();

                    // Hide panels and clear their fields
                    pnlLecturer.Visible = false;
                    txtDepartment.Text = string.Empty;
                    txtSpecialization.Text = string.Empty;

                    pnlStudent.Visible = false;
                    if (ddlProgramme != null) ddlProgramme.ClearSelection();
                    txtIntakeYear.Text = string.Empty;
                    if (ddlAdmissionStatus != null && ddlAdmissionStatus.Items.Count > 0) ddlAdmissionStatus.SelectedIndex = 0;

                    // Refresh the displayed users list
                    LoadUsers(rblSearchRole.SelectedValue);
                }

                catch (Exception ex)
                {
                    transaction.Rollback();

                    lblMessage.Text =
                        ex.Message;
                }
            }
        }
    }
} 