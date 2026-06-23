using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SoftwareEngineeringProject
{
    public partial class AttendanceReport : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CollegeManagementSystem"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RoleID"] == null || Convert.ToInt32(Session["RoleID"]) != 2)
            {
                Response.Redirect("LecturerLogin.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["SelectedCourseID"] == null)
                {
                    Response.Redirect("LecturerDashboard.aspx");
                }

                LoadCourseHeader();
                LoadAttendance();
            }
        }

        private void LoadCourseHeader()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT CourseCode, CourseName FROM Courses WHERE CourseID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(Session["SelectedCourseID"]));

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblCourse.Text = dr["CourseCode"] + " : " + dr["CourseName"];
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        private void LoadAttendance()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {

                string query = @"
                    SELECT S.StudentID AS [Student ID], U.FullName AS [Student Name], U.Email AS [Email],
                    SUM(CASE WHEN A.Status = 'Absent' THEN 1 ELSE 0 END) AS [Total Absences]

                    FROM Enrollments E

                    INNER JOIN Students S ON E.StudentID = S.StudentID
                    INNER JOIN Users U ON S.UserID = U.UserID

                    LEFT JOIN Attendance A ON E.EnrollmentID = A.EnrollmentID
                    WHERE E.CourseID = @CourseID";

                if (!string.IsNullOrEmpty(txtDate.Text))
                {
                    query += " AND (A.AttendanceDate = @Date OR A.AttendanceDate IS NULL)";
                }

                query += @" 
                    GROUP BY S.StudentID, U.FullName, U.Email
                    ORDER BY [Total Absences] ASC, U.FullName ASC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(Session["SelectedCourseID"]));

                if (!string.IsNullOrEmpty(txtDate.Text))
                {
                    cmd.Parameters.AddWithValue("@Date", txtDate.Text);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblNoStudents.Text = "No records found.";
                    gvAttendance.Visible = false;
                    btnPDF.Visible = false;
                }
                else
                {
                    lblNoStudents.Text = "";
                    gvAttendance.Visible = true;
                    btnPDF.Visible = true;

                    gvAttendance.DataSource = dt;
                    gvAttendance.DataBind();
                }

                ViewState["AttendanceData"] = dt;
            }
        }

        protected void gvAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int totalAbsences = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Total Absences"));

                Button btnWarning = (Button)e.Row.FindControl("btnWarning");

                if (totalAbsences >= 4)
                {
                    e.Row.BackColor = System.Drawing.Color.LightCoral;
                    e.Row.ForeColor = System.Drawing.Color.Black;

                    if (btnWarning != null)
                    {
                        btnWarning.Visible = true; 
                    }
                }
                else
                {
                    if (btnWarning != null)
                    {
                        btnWarning.Visible = false;
                    }
                }
            }
        }

        protected void btnWarning_Click(object sender, EventArgs e)
        {
            // TODO: Handle warning logic here (e.g., sending emails or saving system alerts)
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["AttendanceData"];
            if (dt == null || dt.Rows.Count == 0) return;

            Document pdfDoc = new Document(PageSize.A4, 25f, 25f, 30f, 30f);
            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(pdfDoc, ms);

            pdfDoc.Open();

            iTextSharp.text.Font titleFont = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font metaFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC);
            iTextSharp.text.Font thFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font tdFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);

            pdfDoc.Add(new Paragraph("Attendance Summary Report", titleFont));
            pdfDoc.Add(new Paragraph("Course: " + lblCourse.Text, tdFont));
            pdfDoc.Add(new Paragraph("Generated On: " + DateTime.Now.ToString("g"), metaFont));
            pdfDoc.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.WidthPercentage = 100;

            foreach (DataColumn col in dt.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(col.ColumnName, thFont));
                cell.BackgroundColor = new BaseColor(240, 240, 240);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
            }

            foreach (DataRow row in dt.Rows)
            {
                int totalAbsences = Convert.ToInt32(row["Total Absences"]);

                foreach (var item in row.ItemArray)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(item.ToString(), tdFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;

                    if (totalAbsences > 4)
                    {
                        cell.BackgroundColor = new BaseColor(255, 182, 193);
                    }

                    table.AddCell(cell);
                }
            }

            pdfDoc.Add(table);
            pdfDoc.Close();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=AttendanceSummary_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}