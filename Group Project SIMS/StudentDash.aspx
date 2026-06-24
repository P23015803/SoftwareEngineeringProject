<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentDash.aspx.cs" Inherits="Group_Project_SIMS.StudentDash" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Dashboard</title>
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
            <h1>Welcome to the Student Dashboard</h1>
            <p>This is a placeholder for the student dashboard content.</p>
        </div>
    </form>
</body>
</html>
