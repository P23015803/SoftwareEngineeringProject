<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="Group_Project_SIMS.Attendance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attendance - Student Management System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body style="background-color:#f5f6fa;">
    <form id="form1" runat="server">
        <!-- NAVBAR -->
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

        <div class="container mt-4">
            <div class="card shadow p-4">
                <h3 class="mb-0">Attendance</h3>
                <p class="text-muted">View and filter attendance records.</p>

                <div class="row mb-3 g-2">
                    <div class="col-md-3">
                        <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" Placeholder="Search by student name" />
                    </div>

                    <div class="col-md-2">
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>

                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlProgrammeFilter" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">All Programmes</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">All Statuses</asp:ListItem>
                            <asp:ListItem Value="Present">Present</asp:ListItem>
                            <asp:ListItem Value="Absent">Absent</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2 d-flex">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary me-2" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
                    </div>
                </div>

                <asp:Label ID="lblMessage" runat="server"></asp:Label>

                <asp:GridView ID="gvAttendance" runat="server" CssClass="table table-bordered table-striped mt-3" AutoGenerateColumns="false" EmptyDataText="No attendance records found">
                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
                        <asp:BoundField DataField="StudentID" HeaderText="Student ID" ReadOnly="True" />
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" ReadOnly="True" />
                        <asp:BoundField DataField="ProgrammeName" HeaderText="Programme" ReadOnly="True" />
                        <asp:BoundField DataField="ProgrammeHOPName" HeaderText="Programme HOP" ReadOnly="True" />
                        <asp:BoundField DataField="AttendanceDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" ReadOnly="True" />
                        <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="True" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>

    </form>
</body>
</html>
