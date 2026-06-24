﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="Group_Project_SIMS.Course" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Courses</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body>

<form id="form1" runat="server">

<div class="container mt-5">

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

    <h3>Manage Courses</h3>

    <!-- Course Name -->
    <div class="mb-3">
        <label>Course Name</label>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <!-- Course Code -->
    <div class="mb-3">
        <label>Course Code</label>
        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <!-- Credit Hours -->
    <div class="mb-3">
        <label>Credit Hours</label>
        <asp:TextBox ID="txtCredit" runat="server" CssClass="form-control" TextMode ="Number"></asp:TextBox>
    </div>

    <!-- Programme Dropdown -->
    <div class="mb-3">
        <label>Select Programme</label>
        <asp:DropDownList ID="ddlProgramme" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>

    <!-- Lecturer Dropdown -->
    <div class="mb-3">
        <label>Assign Lecturer</label>
        <asp:DropDownList ID="ddlLecturer" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>

    <!-- Save Button -->
    <asp:Button ID="btnSave" runat="server"
        Text="Save Course"
        CssClass="btn btn-primary"
        OnClick="btnSave_Click" />

    <br /><br />

    <asp:Label ID="lblMsg" runat="server"></asp:Label>

    <hr />

    <!-- SEARCH / FILTER -->
    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtSearchCourse" runat="server" CssClass="form-control" Placeholder="Search by Course Name or Code"></asp:TextBox>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlFilterProgramme" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnSearchCourse" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearchCourse_Click" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnResetCourse" runat="server" Text="Reset" CssClass="btn btn-secondary w-100" OnClick="btnResetCourse_Click" />
        </div>
    </div>

    <!-- Course List -->
    <h4>Course List</h4>

    <asp:GridView ID="gvCourse" runat="server"
        CssClass="table table-bordered"
        AutoGenerateColumns="False"
        DataKeyNames="CourseID,ProgrammeID,LecturerID"
        OnRowEditing="gvCourse_RowEditing"
        OnRowUpdating="gvCourse_RowUpdating"
        OnRowCancelingEdit="gvCourse_RowCancelingEdit"
        OnRowDeleting="gvCourse_RowDeleting"
        OnRowDataBound="gvCourse_RowDataBound">

        <Columns>

            <asp:BoundField DataField="CourseID" HeaderText="ID" ReadOnly="True" />

            <asp:TemplateField HeaderText="Course Name">
                <ItemTemplate>
                    <%# Eval("CourseName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control" Text='<%# Bind("CourseName") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Course Code">
                <ItemTemplate>
                    <%# Eval("CourseCode") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEditCode" runat="server" CssClass="form-control" Text='<%# Bind("CourseCode") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Credit Hours">
                <ItemTemplate>
                    <%# Eval("CreditHours") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEditCredit" runat="server" CssClass="form-control" TextMode="Number" Text='<%# Bind("CreditHours") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Programme">
                <ItemTemplate>
                    <%# Eval("ProgrammeName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlEditProgramme" runat="server" CssClass="form-control"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Assigned Lecturer">
                <ItemTemplate>
                    <%# Eval("LecturerName") ?? "" %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlEditLecturer" runat="server" CssClass="form-control"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Action">

                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandName="Edit"
                        Text="Edit" CssClass="btn btn-warning btn-sm me-2" />
                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete this course?');" />
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:LinkButton runat="server" CommandName="Update"
                        Text="Update" CssClass="btn btn-success btn-sm me-2" />

                    <asp:LinkButton runat="server" CommandName="Cancel"
                        Text="Cancel" CssClass="btn btn-secondary btn-sm" />
                </EditItemTemplate>

            </asp:TemplateField>

        </Columns>

    </asp:GridView>

</div>

</form>

</body>
</html>