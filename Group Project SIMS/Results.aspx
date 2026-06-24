<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="Group_Project_SIMS.Results" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Results - Student Management System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
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
                <h3 class="mb-0">Results</h3>
                <p class="text-muted">View student results below. Use filters and search to narrow results.</p>

                <div class="row mb-3 g-2">
                    <div class="col-md-3">
                        <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" Placeholder="Search by student name" AutoPostBack="false" />
                    </div>

                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlSemester" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">All Semesters</asp:ListItem>
                            <asp:ListItem Value="Semester 1">Semester 1</asp:ListItem>
                            <asp:ListItem Value="Semester 2">Semester 2</asp:ListItem>
                            <asp:ListItem Value="Semester 3">Semester 3</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlProgrammeFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlProgrammeFilter_SelectedIndexChanged">
                            <asp:ListItem Value="">All Programmes</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlCourseFilter" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">All Courses</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-1">
                        <asp:DropDownList ID="ddlHOPFilter" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">All HOPs</asp:ListItem>
                            <asp:ListItem Value="1">HOPs</asp:ListItem>
                            <asp:ListItem Value="0">Normal</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlLecturerFilter" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">All Lecturers</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-2">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary w-100" OnClick="btnReset_Click" />
                    </div>
                    <div class="col-md-8 text-end">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </div>
                </div>
                <!-- Additional filters: Grade and Pass/Fail -->
                <div class="row mb-3 g-2">
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlGradeFilter" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">All Grades</asp:ListItem>
                            <asp:ListItem Value="A+">A+</asp:ListItem>
                            <asp:ListItem Value="A">A</asp:ListItem>
                            <asp:ListItem Value="A-">A-</asp:ListItem>
                            <asp:ListItem Value="B+">B+</asp:ListItem>
                            <asp:ListItem Value="B">B</asp:ListItem>
                            <asp:ListItem Value="B-">B-</asp:ListItem>
                            <asp:ListItem Value="C+">C+</asp:ListItem>
                            <asp:ListItem Value="C">C</asp:ListItem>
                            <asp:ListItem Value="C-">C-</asp:ListItem>
                            <asp:ListItem Value="F">F</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlPassFailFilter" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">All</asp:ListItem>
                            <asp:ListItem Value="Passing">Passing</asp:ListItem>
                            <asp:ListItem Value="Failing">Failing</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <asp:GridView ID="gvResults" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" EmptyDataText="No results found">
                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
                        <asp:BoundField DataField="StudentID" HeaderText="Student ID" ReadOnly="True" />
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" ReadOnly="True" />
                        <asp:BoundField DataField="CourseName" HeaderText="Course" ReadOnly="True" />
                        <asp:BoundField DataField="Semester" HeaderText="Semester" ReadOnly="True" />
                        <asp:BoundField DataField="ProgrammeName" HeaderText="Programme" ReadOnly="True" />
                        <asp:BoundField DataField="Marks" HeaderText="Marks" ReadOnly="True" />
                        <asp:BoundField DataField="Grade" HeaderText="Grade" ReadOnly="True" />
                        <asp:BoundField DataField="GradePoint" HeaderText="Grade Point" ReadOnly="True" />
                        <asp:BoundField DataField="CGPA" HeaderText="CGPA" ReadOnly="True" />
                        <asp:BoundField DataField="CourseLecturerName" HeaderText="Course Lecturer" ReadOnly="True" />
                        <asp:BoundField DataField="ProgrammeHOPName" HeaderText="Programme HOP" ReadOnly="True" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>

    </form>
</body>
</html>
