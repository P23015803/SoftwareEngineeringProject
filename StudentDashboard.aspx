<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentDashboard.aspx.cs" Inherits="SoftwareEngineeringProject.StudentDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Student Dashboard</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body>

<form runat="server">

<div class="container mt-4">

    <h2>Student Dashboard</h2>

    <asp:Button ID="btnLogout" runat="server" Text="Logout"
        CssClass="btn btn-danger float-end"
        OnClick="btnLogout_Click" />

    <hr />

    <!-- PROFILE -->
    <h4>Profile</h4>
    <asp:Label ID="lblName" runat="server" />
    <br />
    <asp:Label ID="lblEmail" runat="server" />

    <hr />

    <!-- ENROLLED COURSES -->
    <h4>Enrolled Courses</h4>
    <asp:GridView ID="gvCourses" runat="server" CssClass="table table-bordered" />

</div>

</form>
</body>
</html>
