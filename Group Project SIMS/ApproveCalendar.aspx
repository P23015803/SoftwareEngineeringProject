<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ApproveCalendar.aspx.cs"
    Inherits="Group_Project_SIMS.ApproveCalendar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Approve Calendar Events</title>

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

    <h2>Approve Academic Calendar Events</h2>

    <hr />

    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtSearchTitle" runat="server" CssClass="form-control" Placeholder="Search by title"></asp:TextBox>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlFilterType" runat="server" CssClass="form-select">
                <asp:ListItem>Filter by type</asp:ListItem>
                <asp:ListItem>Registration</asp:ListItem>
                <asp:ListItem>Examination</asp:ListItem>
                <asp:ListItem>Holiday</asp:ListItem>
                <asp:ListItem>Graduation</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlFilterSemester" runat="server" CssClass="form-select">
                <asp:ListItem>Filter by semester</asp:ListItem>
                <asp:ListItem>Semester 1</asp:ListItem>
                <asp:ListItem>Semester 2</asp:ListItem>
                <asp:ListItem>Semester 3</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary w-100" OnClick="btnFilter_Click" />
            <asp:Button ID="btnResetFilter" runat="server" Text="Reset" CssClass="btn btn-secondary w-100 mt-2" OnClick="btnResetFilter_Click" />
        </div>
    </div>

    <asp:GridView ID="gvEvents"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-bordered table-striped"
        OnRowCommand="gvEvents_RowCommand">

        <Columns>

            <asp:BoundField DataField="CalendarID"
                HeaderText="ID" />

            <asp:BoundField DataField="EventTitle"
                HeaderText="Event Title" />

            <asp:BoundField DataField="EventDate"
                HeaderText="Date"
                DataFormatString="{0:dd/MM/yyyy}" />

            <asp:BoundField DataField="EventType"
                HeaderText="Type" />

            <asp:BoundField DataField="Semester"
                HeaderText="Semester" />

            <asp:BoundField DataField="Status"
                HeaderText="Status" />

            <asp:BoundField DataField="CreatedByName"
                HeaderText="Created by" />

            <asp:ButtonField
                Text="Approve"
                CommandName="Approve"
                ButtonType="Button" />

            <asp:ButtonField
                Text="Reject"
                CommandName="Reject"
                ButtonType="Button" />

            <asp:ButtonField
                Text="Publish"
                CommandName="Publish"
                ButtonType="Button" />

        </Columns>

    </asp:GridView>

    <br />

    <asp:Label ID="lblMessage"
        runat="server"
        CssClass="text-success">
    </asp:Label>

</div>

</form>

</body>

</html>