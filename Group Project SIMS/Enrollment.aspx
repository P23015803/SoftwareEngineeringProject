<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Enrollment.aspx.cs" Inherits="Group_Project_SIMS.Enrollment" MaintainScrollPositionOnPostBack="true"%>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Enrollment Management</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>

<body style="background-color:#f5f6fa;">
<form id="form1" runat="server">

    <!-- NAVBAR -->
    <nav class="navbar navbar-dark bg-dark px-4">
        <span class="navbar-brand mb-0 h1">Student Management System</span>
        <a href="AdminDash.aspx" class="btn btn-danger">Back</a>
    </nav>

    <div class="container mt-4">
        <div class="card shadow p-4">
            <h4 class="mb-3">📚 Manage Enrollments</h4>

            <!-- SEARCH CONTROLS -->
            <div class="row mb-3 g-2">
                <div class="col-md-3">
                    <label class="form-label">Search by name</label>
                    <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" Placeholder="Who are you looking for?" />
                </div>

                <div class="col-md-3">
                    <label class="form-label">Sort by Course</label>
                    <asp:DropDownList ID="ddlCourseFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click">
                        <asp:ListItem Value="">All Courses</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <label class="form-label">Sort by Programme</label>
                    <asp:DropDownList ID="ddlProgrammeFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click">
                        <asp:ListItem Value="">All Programmes</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-2">
                    <label class="form-label">Sort by Enrollment Status</label>
                    <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click">
                        <asp:ListItem Value="">All Statuses</asp:ListItem>
                        <asp:ListItem Value="Pending">Pending</asp:ListItem>
                        <asp:ListItem Value="Accepted">Accepted</asp:ListItem>
                        <asp:ListItem Value="Rejected">Rejected</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-1 d-flex align-items-end">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-2">
                    <asp:LinkButton ID="btnToggleSort" runat="server" CssClass="btn btn-link" OnClick="ToggleSort_Click">Sort ↑</asp:LinkButton>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
                </div>
                <div class="col-md-8 text-end">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
            </div>

            <!-- GRID -->
            <asp:GridView ID="gvEnrollments" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" EmptyDataText="No enrollments found" DataKeyNames="EnrollmentID,StudentID,CourseID" OnRowCommand="gvEnrollments_RowCommand">
                <Columns>
                    <asp:BoundField DataField="EnrollmentID" HeaderText="Enrollment ID" ReadOnly="True" />
                    <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
                    <asp:BoundField DataField="FullName" HeaderText="Name" ReadOnly="True" />
                    <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="True" />
                    <asp:BoundField DataField="StudentID" HeaderText="Student ID" ReadOnly="True" />
                    <asp:BoundField DataField="CourseName" HeaderText="Course Name" ReadOnly="True" />
                    <asp:BoundField DataField="CourseCode" HeaderText="Course Code" ReadOnly="True" />
                    <asp:BoundField DataField="ProgrammeName" HeaderText="Programme" ReadOnly="True" />
                    <asp:BoundField DataField="ProgrammeCode" HeaderText="Programme Code" ReadOnly="True" />
                    <asp:BoundField DataField="IntakeYear" HeaderText="Intake Year" ReadOnly="True" />
                    <asp:BoundField DataField="EnrollmentStatus" HeaderText="Enrollment Status" ReadOnly="True" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkAccept" runat="server" CssClass="btn btn-success btn-sm me-1" CommandName="Accept" CommandArgument='<%# Eval("EnrollmentID") %>'>Enroll</asp:LinkButton>
                            <asp:LinkButton ID="lnkReject" runat="server" CssClass="btn btn-danger btn-sm" CommandName="Reject" CommandArgument='<%# Eval("EnrollmentID") %>'>Reject</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </div>
    </div>

</form>
</body>
</html>
 