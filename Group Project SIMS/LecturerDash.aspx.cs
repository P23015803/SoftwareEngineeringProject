using System;
using System.Web.UI;

namespace Group_Project_SIMS
{
    public partial class LecturerDash : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Role verification: Admin only (RoleID == 1)
            if (Session["RoleID"] == null || Convert.ToInt32(Session["RoleID"]) != 1)
            {
                Response.Redirect("Admin Login.aspx");
                return;
            }

            // ...existing Page_Load logic...
        }

        // ...existing code...
    }
}