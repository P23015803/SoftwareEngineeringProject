CREATE DATABASE CollegeManagementSystem;

USE CollegeManagementSystem;

CREATE TABLE Roles
(
    RoleID INT PRIMARY KEY IDENTITY(1,1),

    RoleName VARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO Roles (RoleName)
VALUES
('Admin'),
('Lecturer'),
('Student');

CREATE TABLE Users
(
    UserID INT PRIMARY KEY IDENTITY(1,1),

    FullName VARCHAR(100) NOT NULL,

    Email VARCHAR(100) NOT NULL UNIQUE,

    PasswordHash VARCHAR(255) NOT NULL,

    RoleID INT NOT NULL,

    AccountStatus VARCHAR(20) DEFAULT 'Active',

    CreatedDate DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (RoleID)
    REFERENCES Roles(RoleID)
);

CREATE TABLE Programmes
(
    ProgrammeID INT PRIMARY KEY IDENTITY(1,1),

    ProgrammeName VARCHAR(150) NOT NULL,

    Faculty VARCHAR(100),

    DurationYears INT
);

CREATE TABLE Lecturers
(
    LecturerID INT PRIMARY KEY IDENTITY(1,1),

    UserID INT NOT NULL UNIQUE,

    Department VARCHAR(100),

    Specialization VARCHAR(100),

    HireDate DATE,

    FOREIGN KEY (UserID)
    REFERENCES Users(UserID)
);

CREATE TABLE Students
(
    StudentID INT PRIMARY KEY IDENTITY(1,1),

    UserID INT NOT NULL UNIQUE,

    ProgrammeID INT,

    IntakeYear INT,

    AdmissionStatus VARCHAR(50),

    FOREIGN KEY (UserID)
    REFERENCES Users(UserID),

    FOREIGN KEY (ProgrammeID)
    REFERENCES Programmes(ProgrammeID)
);

CREATE TABLE Courses
(
    CourseID INT PRIMARY KEY IDENTITY(1,1),

    CourseCode VARCHAR(20) NOT NULL UNIQUE,

    CourseName VARCHAR(150) NOT NULL,

    CreditHours INT NOT NULL,

    ProgrammeID INT,

    LecturerID INT,

    FOREIGN KEY (ProgrammeID)
    REFERENCES Programmes(ProgrammeID),

    FOREIGN KEY (LecturerID)
    REFERENCES Lecturers(LecturerID)
);

CREATE TABLE Enrollments
(
    EnrollmentID INT PRIMARY KEY IDENTITY(1,1),

    StudentID INT NOT NULL,

    CourseID INT NOT NULL,

    Semester VARCHAR(50),

    AcademicYear INT,

    EnrollmentStatus VARCHAR(30),

    FOREIGN KEY (StudentID)
    REFERENCES Students(StudentID),

    FOREIGN KEY (CourseID)
    REFERENCES Courses(CourseID)
);

CREATE TABLE Results
(
    ResultID INT PRIMARY KEY IDENTITY(1,1),

    EnrollmentID INT NOT NULL UNIQUE,

    Marks DECIMAL(5,2),

    Grade VARCHAR(5),

    GradePoint DECIMAL(3,2),

    FOREIGN KEY (EnrollmentID)
    REFERENCES Enrollments(EnrollmentID)
);

CREATE TABLE Attendance
(
    AttendanceID INT PRIMARY KEY IDENTITY(1,1),

    EnrollmentID INT NOT NULL,

    AttendanceDate DATE NOT NULL,

    Status VARCHAR(20),

    FOREIGN KEY (EnrollmentID)
    REFERENCES Enrollments(EnrollmentID)
);

CREATE TABLE Announcements
(
    AnnouncementID INT PRIMARY KEY IDENTITY(1,1),

    Title VARCHAR(200) NOT NULL,

    Content TEXT,

    CreatedBy INT,

    CreatedDate DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (CreatedBy)
    REFERENCES Users(UserID)
);

INSERT INTO Users
(
    FullName,
    Email,
    PasswordHash,
    RoleID
)
VALUES
(
    'First Lecturer',
    'lecturer@gmail.com',
    'lecturer123',
    2
);

SELECT * FROM Users;
SELECT * FROM Roles;
SELECT * FROM Lecturers;
SELECT * FROM Attendance;

/*ALTER TABLE*/
ALTER TABLE Results
ADD AssignmentMarks DECIMAL(5,2) DEFAULT 0;

ALTER TABLE Results
ADD QuizMarks DECIMAL(5,2) DEFAULT 0;

ALTER TABLE Results
ADD MidTestMarks DECIMAL(5,2) DEFAULT 0;

ALTER TABLE Results
ADD FinalExamMarks DECIMAL(5,2) DEFAULT 0;

/*DELETE*/
DELETE FROM Attendance;
DELETE FROM Results;
DELETE FROM Enrollments;
DELETE FROM Courses;
DELETE FROM Students;
DELETE FROM Lecturers;
DELETE FROM Announcements;
DELETE FROM Roles;
DELETE FROM Users;
DBCC CHECKIDENT ('Attendance', RESEED, 0);
DBCC CHECKIDENT ('Results', RESEED, 0);
DBCC CHECKIDENT ('Enrollments', RESEED, 0);
DBCC CHECKIDENT ('Courses', RESEED, 0);
DBCC CHECKIDENT ('Students', RESEED, 0);
DBCC CHECKIDENT ('Lecturers', RESEED, 0);
DBCC CHECKIDENT ('Announcements', RESEED, 0);
DBCC CHECKIDENT ('Roles', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);

/*NEW INFO*/
INSERT INTO Lecturers
(
    UserID,
    Department,
    Specialization,
    HireDate
)
VALUES
(
    1,
    'Computer Science',
    'Programming',
    '2025-01-01'
);

INSERT INTO Programmes
(
    ProgrammeName,
    Faculty,
    DurationYears
)
VALUES
(
    'Diploma in Information Technology',
    'Computing',
    3
);

INSERT INTO Courses
(
    CourseCode,
    CourseName,
    CreditHours,
    ProgrammeID,
    LecturerID
)
VALUES
(
    'CSC101',
    'Programming Fundamentals',
    3,
    1,
    1
);

INSERT INTO Users
(
    FullName,
    Email,
    PasswordHash,
    RoleID
)
VALUES
(
    'John Student',
    'student@gmail.com',
    'student123',
    3
);

INSERT INTO Students
(
    UserID,
    ProgrammeID,
    IntakeYear,
    AdmissionStatus
)
VALUES
(
    2,
    1,
    2026,
    'Active'
);

INSERT INTO Enrollments
(
    StudentID,
    CourseID,
    Semester,
    AcademicYear,
    EnrollmentStatus
)
VALUES
(
    1,
    1,
    'Semester 1',
    2026,
    'Active'
);