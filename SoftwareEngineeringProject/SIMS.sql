CREATE DATABASE SIMS;

USE SIMS;

-- ============================================================================
-- 1. BASE INDEPENDENT TABLES
-- ============================================================================

CREATE TABLE Roles (
    RoleID int IDENTITY(1,1) NOT NULL,
    RoleName varchar(50) NULL,
    PRIMARY KEY CLUSTERED (RoleID ASC)
);

-- ============================================================================
-- 2. CORE USERS SYSTEM 
-- ============================================================================

CREATE TABLE Users (
    UserID int IDENTITY(1,1) NOT NULL,
    FullName varchar(100) NOT NULL,
    Email varchar(100) NOT NULL,
    PasswordHash varchar(255) NOT NULL,
    RoleID int NOT NULL,
    AccountStatus varchar(20) DEFAULT ('Active') NULL,
    CreatedDate datetime DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED (UserID ASC),
    UNIQUE NONCLUSTERED (Email ASC),
    FOREIGN KEY(RoleID) REFERENCES Roles (RoleID)
);

-- ============================================================================
-- 3. ADMINISTRATIVE & COMMUNICATIONS LAYERS
-- ============================================================================

CREATE TABLE AcademicCalendar (
    CalendarID int IDENTITY(1,1) NOT NULL,
    EventTitle varchar(150) NULL,
    EventDescription varchar(500) NULL,
    EventDate date NULL,
    EventType varchar(50) NULL,
    Semester varchar(20) NULL,
    CreatedBy int NULL,
    Status varchar(20) NULL,
    PublishStatus varchar(20) NULL,
    CreatedDate datetime NULL,
    PRIMARY KEY CLUSTERED (CalendarID ASC),
    CONSTRAINT FK_AcademicCalendar_Users FOREIGN KEY(CreatedBy) REFERENCES Users (UserID)
);

CREATE TABLE Announcements (
    AnnouncementID int IDENTITY(1,1) NOT NULL,
    Title varchar(200) NOT NULL,
    Content text NULL,
    CreatedBy int NULL,
    CreatedDate datetime DEFAULT (getdate()) NULL,
    PublishStatus varchar(20) NULL,
    Status varchar(20) NULL,
    Semester varchar(20) NULL,
    PRIMARY KEY CLUSTERED (AnnouncementID ASC),
    FOREIGN KEY(CreatedBy) REFERENCES Users (UserID)
);

CREATE TABLE Notifications (
    NotificationID int IDENTITY(1,1) NOT NULL,
    UserID int NOT NULL,
    Title varchar(150) NULL,
    Message text NULL,
    SentDate datetime DEFAULT (getdate()) NULL,
    IsRead bit DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED (NotificationID ASC),
    FOREIGN KEY(UserID) REFERENCES Users (UserID)
);

-- ============================================================================
-- 4. STAFF AND DEPARTMENTS MANAGEMENT
-- ============================================================================

CREATE TABLE Lecturers (
    LecturerID int IDENTITY(1,1) NOT NULL,
    UserID int NOT NULL,
    Department varchar(100) NULL,
    Specialization varchar(100) NULL,
    HireDate date NULL,
    HOPprivileges int DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED (LecturerID ASC),
    UNIQUE NONCLUSTERED (UserID ASC),
    FOREIGN KEY(UserID) REFERENCES Users (UserID)
);

CREATE TABLE Programmes (
    ProgrammeID int IDENTITY(1,1) NOT NULL,
    ProgrammeName varchar(150) NOT NULL,
    ProgrammeCode varchar(100) NULL,
    Faculty varchar(100) NULL,                -- Merged from your script
    DurationYears int NULL,
    LecturerID int NULL,
    PRIMARY KEY CLUSTERED (ProgrammeID ASC),
    CONSTRAINT FK_Programmes_Lecturers FOREIGN KEY(LecturerID) REFERENCES Lecturers (LecturerID)
);

-- ============================================================================
-- 5. ACADEMIC ENROLLMENTS AND ATTENDANCE TRACKING
-- ============================================================================

CREATE TABLE Courses (
    CourseID int IDENTITY(1,1) NOT NULL,
    CourseCode varchar(20) NOT NULL,
    CourseName varchar(150) NOT NULL,
    CreditHours int NOT NULL,
    ProgrammeID int NULL,
    LecturerID int NULL,
    PRIMARY KEY CLUSTERED (CourseID ASC),
    UNIQUE NONCLUSTERED (CourseCode ASC),
    FOREIGN KEY(LecturerID) REFERENCES Lecturers (LecturerID),
    FOREIGN KEY(ProgrammeID) REFERENCES Programmes (ProgrammeID)
);

CREATE TABLE Students (
    StudentID int IDENTITY(1,1) NOT NULL,
    UserID int NOT NULL,
    ProgrammeID int NULL,
    IntakeYear int NULL,
    AdmissionStatus varchar(50) NULL,
    CGPA decimal(3, 2) NULL,
    PRIMARY KEY CLUSTERED (StudentID ASC),
    UNIQUE NONCLUSTERED (UserID ASC),
    FOREIGN KEY(ProgrammeID) REFERENCES Programmes (ProgrammeID),
    FOREIGN KEY(UserID) REFERENCES Users (UserID)
);

CREATE TABLE Enrollments (
    EnrollmentID int IDENTITY(1,1) NOT NULL,
    StudentID int NOT NULL,
    CourseID int NOT NULL,
    Semester varchar(50) NULL,
    AcademicYear int NULL,
    EnrollmentStatus varchar(30) NULL,
    PRIMARY KEY CLUSTERED (EnrollmentID ASC),
    FOREIGN KEY(CourseID) REFERENCES Courses (CourseID),
    FOREIGN KEY(StudentID) REFERENCES Students (StudentID)
);

CREATE TABLE Attendance (
    AttendanceID int IDENTITY(1,1) NOT NULL,
    EnrollmentID int NOT NULL,
    AttendanceDate date NOT NULL,
    Status varchar(20) NULL,
    PRIMARY KEY CLUSTERED (AttendanceID ASC),
    FOREIGN KEY(EnrollmentID) REFERENCES Enrollments (EnrollmentID)
);

CREATE TABLE Results (
    ResultID int IDENTITY(1,1) NOT NULL,
    EnrollmentID int NOT NULL,
    Marks decimal(5, 2) NULL,
    Grade varchar(5) NULL,
    GradePoint decimal(3, 2) NULL,
    AssignmentMarks decimal(5, 2) DEFAULT 0 NULL, -- Merged from your updates
    MidTestMarks decimal(5, 2) DEFAULT 0 NULL,    -- Merged from your updates
    FinalExamMarks decimal(5, 2) DEFAULT 0 NULL,  -- Merged from your updates
    PRIMARY KEY CLUSTERED (ResultID ASC),
    UNIQUE NONCLUSTERED (EnrollmentID ASC),
    FOREIGN KEY(EnrollmentID) REFERENCES Enrollments (EnrollmentID)
);

-- ============================================================================
-- 6. EXTRA COURSEWORK SUBMISSIONS LAYERS (Merged from your script)
-- ============================================================================

CREATE TABLE Assignments (
    AssignmentID int IDENTITY(1,1) NOT NULL,
    CourseID int NULL,
    Title varchar(200) NULL,
    DueDate date NULL,
    PRIMARY KEY CLUSTERED (AssignmentID ASC),
    FOREIGN KEY (CourseID) REFERENCES Courses (CourseID)
);

CREATE TABLE AssignmentSubmissions (
    SubmissionID int IDENTITY(1,1) NOT NULL,
    AssignmentID int NULL,
    EnrollmentID int NULL,
    FileName varchar(255) NULL,
    FilePath varchar(500) NULL,
    SubmissionDate datetime NULL,
    Marks decimal(5, 2) NULL,
    PRIMARY KEY CLUSTERED (SubmissionID ASC),
    FOREIGN KEY (AssignmentID) REFERENCES Assignments (AssignmentID),
    FOREIGN KEY (EnrollmentID) REFERENCES Enrollments (EnrollmentID)
);

-- ============================================================================
-- 7. DEFAULT SYSTEM CONFIG DATA DATA INSERTS
-- ============================================================================

INSERT INTO Roles (RoleName) VALUES ('Admin'), ('Lecturer'), ('Student');