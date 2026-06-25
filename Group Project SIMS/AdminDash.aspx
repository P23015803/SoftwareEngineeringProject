<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDash.aspx.cs" Inherits="Group_Project_SIMS.AdminDash" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        /* Grid container for buttons: 3 columns, responsive */
        .button-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 20px;
            max-width: 1100px;
            margin: 20px auto;
            padding: 10px;
            box-sizing: border-box;
        }

        /* Large rounded button look for links */
        .big-button {
            display: flex;
            align-items: center;
            justify-content: center;
            text-decoration: none;
            background: #007bff; /* blue */
            color: #fff;
            font-size: 1.1rem;
            font-weight: 600;
            padding: 22px 18px;
            border-radius: 12px;
            box-shadow: 0 6px 12px rgba(0,0,0,0.08);
            transition: transform 120ms ease, box-shadow 120ms ease, background 120ms ease;
            min-height: 72px;
            text-align: center;
        }

        .big-button:hover, .big-button:focus {
            transform: translateY(-3px);
            box-shadow: 0 10px 20px rgba(0,0,0,0.12);
            background: #0056b3;
        }

        /* Make it stack nicely on smaller screens */
        @media (max-width: 900px) {
            .button-grid { grid-template-columns: repeat(2, 1fr); }
        }

        @media (max-width: 520px) {
            .button-grid { grid-template-columns: 1fr; }
        }

        /* Simple heading layout */
        .page-wrap { max-width: 1100px; margin: 10px auto; padding: 10px; box-sizing: border-box; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-wrap">
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
                         <li class="nav-item"><a class="nav-link text-white" href="AdminAnnouncements.aspx">Admin Announcements</a></li>
                    </ul>
                </div>
            </nav>

            <h1>Welcome to the Admin Dashboard</h1>
            <p>This is a placeholder for the admin dashboard content.</p>
            <p>Reminder: Tweak the code to ensure insertion fields are cleared, that the webpage doesn't shoot back up, and to stop blank or invalid entries.</p>

            <!-- Button grid: add more <a> elements inside .button-grid to create new buttons -->
            <div class="button-grid" role="navigation" aria-label="Admin links">
                <a href="RegisterUsers.aspx" class="big-button">Register Users</a>
                <a href="Programme.aspx" class="big-button">Programme</a>
                <a href="Courses.aspx" class="big-button">Courses</a>
                <a href="ApproveCalendar.aspx" class="big-button">Approve Calendar</a>
                <a href="ManageCalendar.aspx" class="big-button">Manage Calendar</a>
                <a href="Enrollment.aspx" class="big-button">Enrollment</a>
                <a href="Attendance.aspx" class="big-button">Attendance</a>
                <a href="Results.aspx" class="big-button">Results</a>
                <a href="AdminAnnouncements.aspx" class="big-button">Admin Announcements</a>
                
            </div>
        </div>
    </form>
</body>
</html>
