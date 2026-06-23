using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftwareEngineeringProject
{
    public partial class EnterMarks : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SelectedCourseID"] == null)
                {
                    Response.Redirect("LecturerDashboard.aspx");
                }

                LoadCourseInfo();
                LoadStudents();
            }
        }

        private void LoadCourseInfo()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                string query = @"SELECT CourseCode, CourseName FROM Courses WHERE CourseID = @CourseID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(Session["SelectedCourseID"]));

                conn.Open();

                SqlDataReader dr =
                    cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblCourse.Text = dr["CourseCode"] + " : " + dr["CourseName"];
                }
            }
        }

        private void LoadStudents()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                string query =
                @"SELECT E.EnrollmentID, S.StudentID, U.FullName, R.AssignmentMarks, R.MidTestMarks, R.FinalExamMarks
                
                FROM Enrollments E
                
                LEFT JOIN Results R
                ON E.EnrollmentID = R.EnrollmentID

                INNER JOIN Students S
                ON E.StudentID = S.StudentID

                INNER JOIN Users U
                ON S.UserID = U.UserID

                WHERE E.CourseID = @CourseID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(Session["SelectedCourseID"]));

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text =
                        "No students enrolled in this course.";

                    gvStudents.Visible = false;
                    return;
                }

                gvStudents.Visible = true;

                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
        }

        protected void gvStudents_RowCommand(object sender,GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewAssignments")
            {
                Session["SelectedEnrollmentID"] = Convert.ToInt32(e.CommandArgument);

                Response.Redirect("StudentAssignments.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                foreach (GridViewRow row in gvStudents.Rows)
                {
                    int enrollmentID = Convert.ToInt32(gvStudents.DataKeys[row.RowIndex].Value);

                    decimal assignment = 0;

                    string assignmentQuery = 
                        @"
                        SELECT ISNULL(SUM(Marks),0)
                        FROM AssignmentSubmissions
                        WHERE EnrollmentID = @EnrollmentID";

                    SqlCommand assignmentCmd = new SqlCommand(assignmentQuery, conn);

                    assignmentCmd.Parameters.AddWithValue("@EnrollmentID",enrollmentID);

                    assignment = Convert.ToDecimal(assignmentCmd.ExecuteScalar());

                    decimal midTest =
                        string.IsNullOrWhiteSpace(((TextBox)row.FindControl("txtMidTest")).Text)
                        ? 0
                        : Convert.ToDecimal(((TextBox)row.FindControl("txtMidTest")).Text);

                    decimal finalExam =
                        string.IsNullOrWhiteSpace(((TextBox)row.FindControl("txtFinalExam")).Text)
                        ? 0
                        : Convert.ToDecimal(((TextBox)row.FindControl("txtFinalExam")).Text);

                    decimal totalMarks = assignment + midTest + finalExam;

                    string grade;
                    decimal gradePoint;

                    if (totalMarks >= 80)
                    {
                        grade = "A";
                        gradePoint = 4.00m;
                    }
                    else if (totalMarks >= 70)
                    {
                        grade = "B";
                        gradePoint = 3.00m;
                    }
                    else if (totalMarks >= 60)
                    {
                        grade = "C";
                        gradePoint = 2.00m;
                    }
                    else if (totalMarks >= 50)
                    {
                        grade = "D";
                        gradePoint = 1.00m;
                    }
                    else
                    {
                        grade = "F";
                        gradePoint = 0.00m;
                    }

                    string query =
                    @"IF EXISTS
                    (
                        SELECT 1
                        FROM Results
                        WHERE EnrollmentID = @EnrollmentID
                    )
                    BEGIN
                        UPDATE Results
                        SET AssignmentMarks = @Assignment,
                            MidTestMarks = @MidTest,
                            FinalExamMarks = @FinalExam,
                            Marks = @Marks,
                            Grade = @Grade,
                            GradePoint = @GradePoint
                        WHERE EnrollmentID = @EnrollmentID
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Results
                        (
                            EnrollmentID,
                            AssignmentMarks,
                            MidTestMarks,
                            FinalExamMarks,
                            Marks,
                            Grade,
                            GradePoint
                        )
                        VALUES
                        (
                            @EnrollmentID,
                            @Assignment,
                            @MidTest,
                            @FinalExam,
                            @Marks,
                            @Grade,
                            @GradePoint
                        )
                    END";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentID);

                    cmd.Parameters.AddWithValue("@Assignment", assignment);

                    cmd.Parameters.AddWithValue("@MidTest", midTest);

                    cmd.Parameters.AddWithValue("@FinalExam", finalExam);

                    cmd.Parameters.AddWithValue("@Marks", totalMarks);

                    cmd.Parameters.AddWithValue("@Grade", grade);

                    cmd.Parameters.AddWithValue("@GradePoint", gradePoint);

                    cmd.ExecuteNonQuery();
                }

                lblMessage.Text = "Marks saved successfully.";
            }
        }
    }
}