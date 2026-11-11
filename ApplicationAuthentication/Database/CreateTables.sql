
USE [CentralAuthenticationDB];
GO

/* =============================================
   1. APPLICATIONS
   ============================================= */
CREATE TABLE dbo.Applications
(
    ApplicationId NVARCHAR(100) NOT NULL,
    ApplicationName NVARCHAR(150) NOT NULL,
    HashKey NVARCHAR(500) NOT NULL,
    IsActive BIT NOT NULL
        CONSTRAINT DF_Applications_IsActive DEFAULT (1),
    CreatedAt DATETIME2(7) NOT NULL
        CONSTRAINT DF_Applications_CreatedAt DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Applications
        PRIMARY KEY (ApplicationId)
);
GO

/* =============================================
   2. USERS
   ============================================= */
CREATE TABLE dbo.Users
(
    UserId NVARCHAR(50) NOT NULL,
    UserName NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    IsActive BIT NOT NULL
        CONSTRAINT DF_Users_IsActive DEFAULT (1),
    IsBlocked BIT NOT NULL
        CONSTRAINT DF_Users_IsBlocked DEFAULT (0),
    CreatedAt DATETIME2(7) NOT NULL
        CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Users
        PRIMARY KEY (UserId)
);
GO

/* =============================================
   3. GROUPS
   ============================================= */
CREATE TABLE dbo.Groups
(
    GroupId INT IDENTITY(1,1) NOT NULL,
    GroupName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(250) NULL,
    IsActive BIT NOT NULL
        CONSTRAINT DF_Groups_IsActive DEFAULT (1),
    CreatedAt DATETIME2(7) NOT NULL
        CONSTRAINT DF_Groups_CreatedAt DEFAULT (SYSUTCDATETIME()),
    ApplicationId NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_Groups
        PRIMARY KEY (GroupId),

    CONSTRAINT UQ_Groups_Application_Group
        UNIQUE (ApplicationId, GroupId),

    CONSTRAINT UQ_Groups_Application_GroupName
        UNIQUE (ApplicationId, GroupName),

    CONSTRAINT FK_Groups_Applications
        FOREIGN KEY (ApplicationId)
        REFERENCES dbo.Applications(ApplicationId)
);
GO

/* =============================================
   4. PERMISSIONS
   ============================================= */
CREATE TABLE dbo.Permissions
(
    PermissionId INT IDENTITY(1,1) NOT NULL,
    PermissionCode NVARCHAR(100) NOT NULL,
    PermissionName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    IsActive BIT NOT NULL
        CONSTRAINT DF_Permissions_IsActive DEFAULT (1),
    CreatedAt DATETIME2(7) NOT NULL
        CONSTRAINT DF_Permissions_CreatedAt DEFAULT (SYSUTCDATETIME()),
    ApplicationId NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_Permissions
        PRIMARY KEY (PermissionId),

    CONSTRAINT UQ_Permissions_Application_Code
        UNIQUE (ApplicationId, PermissionCode),

    CONSTRAINT UQ_Permissions_Application_Permission
        UNIQUE (ApplicationId, PermissionId),

    CONSTRAINT FK_Permissions_Applications
        FOREIGN KEY (ApplicationId)
        REFERENCES dbo.Applications(ApplicationId)
);
GO

/* =============================================
   5. USER APPLICATIONS
   ============================================= */
CREATE TABLE dbo.UserApplications
(
    UserId NVARCHAR(50) NOT NULL,
    ApplicationId NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL
        CONSTRAINT DF_UserApplications_IsActive DEFAULT (1),
    AssignedAt DATETIME2(7) NOT NULL
        CONSTRAINT DF_UserApplications_AssignedAt DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_UserApplications
        PRIMARY KEY (UserId, ApplicationId),

    CONSTRAINT FK_UserApplications_Users
        FOREIGN KEY (UserId)
        REFERENCES dbo.Users(UserId),

    CONSTRAINT FK_UserApplications_Applications
        FOREIGN KEY (ApplicationId)
        REFERENCES dbo.Applications(ApplicationId)
);
GO

/* =============================================
   6. USER GROUPS
   ============================================= */
CREATE TABLE dbo.UserGroups
(
    UserId NVARCHAR(50) NOT NULL,
    GroupId INT NOT NULL,
    ApplicationId NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_UserGroups
        PRIMARY KEY (UserId, ApplicationId, GroupId),

    CONSTRAINT FK_UserGroups_Users
        FOREIGN KEY (UserId)
        REFERENCES dbo.Users(UserId),

    CONSTRAINT FK_UserGroups_Groups
        FOREIGN KEY (ApplicationId, GroupId)
        REFERENCES dbo.Groups(ApplicationId, GroupId),

    CONSTRAINT FK_UserGroups_UserApplications
        FOREIGN KEY (UserId, ApplicationId)
        REFERENCES dbo.UserApplications(UserId, ApplicationId)
);
GO

/* =============================================
   7. GROUP PERMISSIONS
   ============================================= */
CREATE TABLE dbo.GroupPermissions
(
    GroupId INT NOT NULL,
    PermissionId INT NOT NULL,
    ApplicationId NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_GroupPermissions
        PRIMARY KEY (ApplicationId, GroupId, PermissionId),

    CONSTRAINT FK_GroupPermissions_Groups
        FOREIGN KEY (ApplicationId, GroupId)
        REFERENCES dbo.Groups(ApplicationId, GroupId),

    CONSTRAINT FK_GroupPermissions_Permissions
        FOREIGN KEY (ApplicationId, PermissionId)
        REFERENCES dbo.Permissions(ApplicationId, PermissionId)
);
GO

PRINT 'All seven authentication tables created successfully.';
GO
