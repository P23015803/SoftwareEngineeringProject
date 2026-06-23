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

    <nav class="navbar navbar-dark bg-dark px-4">
        <span class="navbar-brand mb-0 h1">Student Information Management System</span>
        <a href="AdminDash.aspx" class="btn btn-danger">Back</a>
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