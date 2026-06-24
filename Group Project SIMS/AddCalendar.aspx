<!--NOTE: ADD CALENDAR IS FOR THE LECTURER MODULE-->

<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="AddCalendar.aspx.cs"
    Inherits="Group_Project_SIMS.AddCalendar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Add Academic Calendar Event</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />

</head>

<body>

<form id="form1" runat="server">

<div class="container mt-4">

    <nav class="navbar navbar-expand-lg navbar-dark bg-dark px-4">
        <a class="navbar-brand" href="AdminDash.aspx">Student Management System</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mainNav" aria-controls="mainNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="mainNav">
            <ul class="navbar-nav ms-auto">
                <li class="nav-item"><a class="nav-link text-white" href="AdminDash.aspx">Dashboard</a></li>
                <li class="nav-item"><a class="nav-link text-white" href="RegisterUsers.aspx">Register Users</a></li>
                <li class="nav-item"><a class="nav-link text-white" href="Programme.aspx">Programme</a></li>
                <li class="nav-item"><a class="nav-link text-white" href="Courses.aspx">Courses</a></li>
                <li class="nav-item"><a class="nav-link text-white" href="Enrollment.aspx">Enrollment</a></li>
                <li class="nav-item"><a class="nav-link text-white" href="Attendance.aspx">Attendance</a></li>
                <li class="nav-item"><a class="nav-link text-white" href="Results.aspx">Results</a></li>
                <li class="nav-item"><a class="nav-link text-white" href="ApproveCalendar.aspx">Approve Calendar</a></li>
                <li class="nav-item"><a class="nav-link text-white" href="ManageCalendar.aspx">Manage Calendar</a></li>
            </ul>
        </div>
    </nav>

    <div class="card">

        <div class="card-header bg-primary text-white">

            <h3>Add Academic Calendar Event</h3>

        </div>

        <div class="card-body">

            <div class="mb-3">

                <label>Event Title</label>

                <asp:TextBox ID="txtTitle"
                    runat="server"
                    CssClass="form-control">
                </asp:TextBox>

                <asp:RequiredFieldValidator
                    ID="rfvTitle"
                    runat="server"
                    ControlToValidate="txtTitle"
                    ErrorMessage="Event Title is required"
                    ForeColor="Red">
                </asp:RequiredFieldValidator>

            </div>

            <div class="mb-3">

                <label>Event Description</label>

                <asp:TextBox ID="txtDescription"
                    runat="server"
                    CssClass="form-control"
                    TextMode="MultiLine"
                    Rows="4">
                </asp:TextBox>

            </div>

            <div class="mb-3">

                <label>Event Date</label>

                <asp:TextBox ID="txtDate"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Date">
                </asp:TextBox>

            </div>

            <div class="mb-3">

                <label>Event Type</label>

                <asp:DropDownList ID="ddlType"
                    runat="server"
                    CssClass="form-control">

                    <asp:ListItem>Registration</asp:ListItem>
                    <asp:ListItem>Examination</asp:ListItem>
                    <asp:ListItem>Holiday</asp:ListItem>
                    <asp:ListItem>Graduation</asp:ListItem>
                    <asp:ListItem>Workshop</asp:ListItem>

                </asp:DropDownList>

            </div>

            <div class="mb-3">

                <label>Semester</label>

                <asp:DropDownList ID="ddlSemester"
                    runat="server"
                    CssClass="form-control">

                    <asp:ListItem>Semester 1</asp:ListItem>
                    <asp:ListItem>Semester 2</asp:ListItem>
                    <asp:ListItem>Semester 3</asp:ListItem>

                </asp:DropDownList>

            </div>

            <asp:Button ID="btnSubmit"
                runat="server"
                Text="Submit Event"
                CssClass="btn btn-success"
                OnClick="btnSubmit_Click" />

            <asp:Button ID="btnClear"
                runat="server"
                Text="Clear"
                CssClass="btn btn-secondary"
                OnClick="btnClear_Click" />

            <a href="LecturerDashboard.aspx"
               class="btn btn-primary">
               Back
            </a>

            <br /><br />

            <asp:Label ID="lblMessage"
                runat="server">
            </asp:Label>

        </div>

    </div>

</div>

</form>

</body>
</html>