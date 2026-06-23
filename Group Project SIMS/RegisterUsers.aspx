<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterUsers.aspx.cs" Inherits="Group_Project_SIMS.RegisterUsers" MaintainScrollPositionOnPostBack="true"%>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Register Lecturer</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>

<body style="background-color:#f5f6fa;">
<form id="form1" runat="server" autocomplete="off">

    <!-- NAVBAR -->
    <nav class="navbar navbar-dark bg-dark px-4">
        <span class="navbar-brand mb-0 h1">Student Management System</span>
        <a href="AdminDash.aspx" class="btn btn-danger">Back</a>
    </nav>

    <!-- FORM -->
    <div class="container mt-5 d-flex justify-content-center">
        <div class="card shadow p-4" style="width: 400px;">
            
            <h3 class="text-center mb-4">👩‍🏫 Register Users</h3>

            <asp:Label ID="lblAllFields" runat="server" CssClass="text-danger mb-2" Visible="false">All fields must be filled</asp:Label>
            <div class="mb-3">
                <label class="form-label">Full Name</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
                <label class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" autocomplete="off" />
            </div>

            <div class="mb-3">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" autocomplete="new-password" />
            </div>

            <asp:RadioButtonList
                ID="rblRole"
                runat="server"
                RepeatDirection="Horizontal"
                CssClass="mb-3"
                AutoPostBack="true"
                OnSelectedIndexChanged=
                "rblRole_SelectedIndexChanged">

                <asp:ListItem Value="2">
                    Lecturer
                </asp:ListItem>

                <asp:ListItem Value="3">
                    Student
                </asp:ListItem>

            </asp:RadioButtonList>
            <!-- role validation handled server-side; see lblAllFields message -->

            <!-- lecturer panel -->
            <asp:Panel ID="pnlLecturer" runat="server" Visible="false">

                <div class="mb-3">
                    <label>Department</label>
                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control"/>
                </div>

                <div class="mb-3">
                    <label>Specialization</label>
                    <asp:TextBox ID="txtSpecialization" runat="server" CssClass="form-control"/>
                </div>

                <div class="mb-3 form-check">
                    <asp:CheckBox ID="chkHOP" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label" for="chkHOP">Is this a Head of Programme?</label>
                </div>

            </asp:Panel>

            <!-- student panel -->
            <asp:Panel ID="pnlStudent" runat="server" Visible="false">

                <div class="mb-3">
                    <label>Programme</label>
                    <asp:DropDownList ID="ddlProgramme" runat="server" CssClass="form-select"/>
                </div>

                <div class="mb-3">
                    <label>Intake Year</label>
                    <asp:TextBox ID="txtIntakeYear" runat="server" CssClass="form-control"/>
                </div>

                <div class="mb-3">
                    <label>Admission Status</label>
                    <asp:DropDownList ID="ddlAdmissionStatus" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Pending" Selected="True">Pending</asp:ListItem>
                        <asp:ListItem Value="Accepted">Accepted</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </asp:Panel>


            <div class="d-grid">
                <asp:Button ID="btnRegister" runat="server" Text="Register"
                    CssClass="btn btn-primary"
                    OnClick="btnRegister_Click" />
            </div>

            <div class="mt-3 text-center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>

        </div>
    </div>

    <!-- SEARCH + GRID -->
    <div class="container mt-4">
        <div class="card shadow p-4">

            <h4 class="mb-3">📋 Registered Users</h4>

            <!-- SEARCH -->
            <div class="row mb-3 g-2">
                <div class="col-md-3">
                    <asp:TextBox ID="txtSearchDept" runat="server"
                        CssClass="form-control"
                        Placeholder="Search by name"
                        AutoPostBack="true"
                        OnTextChanged="btnSearch_Click" />
                </div>

                <div class="col-md-2">
                    <asp:DropDownList ID="ddlSearchProgramme" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click">
                        <asp:ListItem Value="">All Programmes</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-2">
                    <asp:DropDownList ID="ddlSearchAdmissionStatus" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click">
                        <asp:ListItem Value="">All Statuses</asp:ListItem>
                        <asp:ListItem Value="Pending">Pending</asp:ListItem>
                        <asp:ListItem Value="Accepted">Accepted</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-2">
                    <asp:DropDownList ID="ddlSearchHOP" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click">
                        <asp:ListItem Value="">All Lecturers</asp:ListItem>
                        <asp:ListItem Value="1">HOPs</asp:ListItem>
                        <asp:ListItem Value="0">Normal Lecturers</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-1">
                    <asp:Button ID="btnSearch" runat="server" Text="Search"
                        CssClass="btn btn-primary w-100"
                        OnClick="btnSearch_Click" />
                </div>

                <div class="col-md-1 d-flex align-items-center">
                    <asp:LinkButton ID="btnToggleSort" runat="server" CssClass="btn btn-link" OnClick="ToggleSort_Click">Sort ↑</asp:LinkButton>
                </div>

                <div class="col-md-1">
                    <asp:Button ID="btnReset" runat="server" Text="Reset"
                        CssClass="btn btn-secondary w-100"
                        OnClick="btnReset_Click" />
                </div>
            </div>

            <!-- GRIDVIEW -->
            <asp:RadioButtonList
                ID="rblSearchRole"
                runat="server"
                AutoPostBack="true"
                OnSelectedIndexChanged="rblSearchRole_SelectedIndexChanged">

                <asp:ListItem Value="Student" Selected="True">
                    Students
                </asp:ListItem>

                <asp:ListItem Value="Lecturer">
                    Lecturers
                </asp:ListItem>

            </asp:RadioButtonList>

            <asp:Panel ID="pnlStudentGrid" runat="server" Visible="true">
                <asp:GridView ID="gvStudents" runat="server"
                    CssClass="table table-bordered table-striped"
                    AutoGenerateColumns="false"
                    EmptyDataText="No users found"
                    DataKeyNames="UserID,ProgrammeID,StudentID"
                    OnRowEditing="gvStudents_RowEditing"
                    OnRowCancelingEdit="gvStudents_RowCancelingEdit"
                    OnRowUpdating="gvStudents_RowUpdating"
                    OnRowDeleting="gvStudents_RowDeleting"
                    OnRowDataBound="gvStudents_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="StudentID" HeaderText="Student ID" ReadOnly="True" />

                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <%# Eval("FullName") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFullName" runat="server" Text='<%# Bind("FullName") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <%# Eval("Email") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Programme">
                            <ItemTemplate>
                                <%# Eval("ProgrammeName") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlGridProgramme" runat="server" CssClass="form-select"></asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Intake Year">
                            <ItemTemplate>
                                <%# Eval("IntakeYear") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtIntakeYearGrid" runat="server" Text='<%# Bind("IntakeYear") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Admission Status">
                            <ItemTemplate>
                                <%# Eval("AdmissionStatus") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlAdmissionStatusGrid" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                    <asp:ListItem Value="Accepted">Accepted</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                    </Columns>

                </asp:GridView>
            </asp:Panel>

            <asp:Panel ID="pnlLecturerGrid" runat="server" Visible="true">
                <asp:GridView ID="gvLecturers" runat="server"
                    CssClass="table table-bordered table-striped"
                    AutoGenerateColumns="false"
                    EmptyDataText="No users found"
                    DataKeyNames="UserID,LecturerID"
                    OnRowEditing="gvLecturers_RowEditing"
                    OnRowCancelingEdit="gvLecturers_RowCancelingEdit"
                    OnRowUpdating="gvLecturers_RowUpdating"
                    OnRowDeleting="gvLecturers_RowDeleting">

                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="LecturerID" HeaderText="Lecturer ID" ReadOnly="True" />

                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <%# Eval("FullName") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLectFullName" runat="server" Text='<%# Bind("FullName") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <%# Eval("Email") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLectEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField HeaderText="Department">
                            <ItemTemplate>
                                <%# Eval("Department") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDepartmentGrid" runat="server" Text='<%# Bind("Department") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField HeaderText="HOP">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkHOPItem" runat="server" Enabled="false" Checked='<%# Eval("HOPprivileges").ToString() == "1" %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkHOPGrid" runat="server" Checked='<%# Eval("HOPprivileges").ToString() == "1" %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>

                        <asp:TemplateField HeaderText="Specialization">
                            <ItemTemplate>
                                <%# Eval("Specialization") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSpecializationGrid" runat="server" Text='<%# Bind("Specialization") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="HireDate" HeaderText="Hire Date" ReadOnly="True" />

                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                    </Columns>

                </asp:GridView>
            </asp:Panel>


        </div>
    </div>

</form>
</body>
</html>
