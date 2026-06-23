﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Programme.aspx.cs" Inherits="Group_Project_SIMS.Programme" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Programme</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body>

<form id="form1" runat="server">

<div class="container mt-5">

    <!-- NAVBAR -->
    <nav class="navbar navbar-dark bg-dark px-4">
        <span class="navbar-brand mb-0 h1">Student Information Management System</span>
        <a href="AdminDash.aspx" class="btn btn-danger">Back</a>
    </nav>

    <h3>Manage Programme</h3>

    <div class="mb-3">
        <label>Programme Name</label>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="mb-3">
        <label>Programme Code</label>
        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="mb-3">
        <label>Duration (Years)</label>
        <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
    </div>

    <div class="mb-3">
        <label>Head of Programme</label>
        <asp:DropDownList ID="ddlLecturer" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>

    <asp:Button ID="btnSave" runat="server"
        Text="Save Programme"
        CssClass="btn btn-primary"
        OnClick="btnSave_Click" />

    <br /><br />

    <asp:Label ID="lblMsg" runat="server"></asp:Label>

    <hr />

    <!-- SEARCH -->
    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtSearchProgramme" runat="server" CssClass="form-control" Placeholder="Search by Programme Name or Code"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnSearchProgramme" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearchProgramme_Click" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnResetProgramme" runat="server" Text="Reset" CssClass="btn btn-secondary w-100" OnClick="btnResetProgramme_Click" />
        </div>
    </div>

    <h4>Programme List</h4>

    <asp:GridView ID="gvProgramme" runat="server"
        CssClass="table table-bordered"
        AutoGenerateColumns="False"
        DataKeyNames="ProgrammeID,LecturerID"
        OnRowEditing="gvProgramme_RowEditing"
        OnRowUpdating="gvProgramme_RowUpdating"
        OnRowCancelingEdit="gvProgramme_RowCancelingEdit"
        OnRowDeleting="gvProgramme_RowDeleting"
        OnRowDataBound="gvProgramme_RowDataBound">

        <Columns>

            <asp:BoundField DataField="ProgrammeID" HeaderText="ID" ReadOnly="True" />

            <asp:BoundField DataField="ProgrammeName" HeaderText="Programme Name" Visible="False" />

            <asp:TemplateField HeaderText="Programme Name">
                <ItemTemplate>
                    <%# Eval("ProgrammeName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control" Text='<%# Bind("ProgrammeName") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Programme Code">
                <ItemTemplate>
                    <%# Eval("ProgrammeCode") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEditCode" runat="server" CssClass="form-control" Text='<%# Bind("ProgrammeCode") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Duration (Years)">
                <ItemTemplate>
                    <%# Eval("DurationYears") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEditDuration" runat="server" CssClass="form-control" TextMode="Number" Text='<%# Bind("DurationYears") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Head of Programme">
                <ItemTemplate>
                    <%# Eval("LecturerName") ?? "" %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlEditLecturer" runat="server" CssClass="form-control"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Action">

                <ItemTemplate>
                    <asp:LinkButton ID="btnEdit" runat="server"
                        CommandName="Edit"
                        Text="Edit"
                        CssClass="btn btn-warning btn-sm me-2" />
                    <asp:LinkButton ID="btnDelete" runat="server"
                        CommandName="Delete"
                        Text="Delete"
                        CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete this programme?');" />
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:LinkButton ID="btnUpdate" runat="server"
                        CommandName="Update"
                        Text="Update"
                        CssClass="btn btn-success btn-sm me-2" />

                    <asp:LinkButton ID="btnCancel" runat="server"
                        CommandName="Cancel"
                        Text="Cancel"
                        CssClass="btn btn-secondary btn-sm" />
                </EditItemTemplate>

            </asp:TemplateField>

        </Columns>

    </asp:GridView>

</div>

</form>

</body>
</html> 