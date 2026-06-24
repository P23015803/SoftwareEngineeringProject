<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ManageCalendar.aspx.cs"
    Inherits="Group_Project_SIMS.ManageCalendar" %>

<!DOCTYPE html>

<html>

<head runat="server">

    <title>Manage Academic Calendar</title>

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

<h2>Academic Calendar Management</h2>

<hr />

<!-- Search and filter controls moved below so they appear just above the grid -->

<asp:HiddenField ID="hfCalendarID"
    runat="server" />

<div class="mb-3">

<label>Event Title</label>

<asp:TextBox ID="txtTitle"
    runat="server"
    CssClass="form-control">
</asp:TextBox>

</div>

<div class="mb-3">

<label>Description</label>

<asp:TextBox ID="txtDescription"
    runat="server"
    TextMode="MultiLine"
    Rows="4"
    CssClass="form-control">
</asp:TextBox>

</div>

<div class="mb-3">

<label>Event Date</label>

<asp:TextBox ID="txtDate"
    runat="server"
    TextMode="Date"
    CssClass="form-control">
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

<div class="mb-3">

<asp:Button ID="btnSave"
    runat="server"
    Text="Save"
    CssClass="btn btn-success"
    OnClick="btnSave_Click" />

<asp:Button ID="btnUpdate"
    runat="server"
    Text="Update"
    CssClass="btn btn-primary"
    OnClick="btnUpdate_Click" />

</div>

<hr />

    <!-- Search/filter row moved here so it appears above the GridView -->
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

    <asp:GridView ID="gvCalendar"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="table table-bordered"
    OnRowCommand="gvCalendar_RowCommand">

    <Columns>

        <asp:BoundField DataField="CalendarID"
            HeaderText="ID" />

        <asp:BoundField DataField="EventTitle"
            HeaderText="Title" />

        <asp:BoundField DataField="EventDescription"
            HeaderText="Description" />

        <asp:BoundField DataField="CreatedByName"
            HeaderText="Created by" />

        <asp:BoundField DataField="EventDate"
            HeaderText="Date" />

        <asp:BoundField DataField="EventType"
            HeaderText="Type" />

        <asp:BoundField DataField="Semester"
            HeaderText="Semester" />

        <asp:ButtonField
            CommandName="SelectRecord"
            Text="Select"
            ButtonType="Button" />

        <asp:ButtonField
            CommandName="DeleteRecord"
            Text="Delete"
            ButtonType="Button" />

    </Columns>

</asp:GridView>

</div>

</form>

</body>

</html>