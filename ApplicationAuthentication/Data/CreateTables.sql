CREATE TABLE Users
(
    UserId NVARCHAR(50) NOT NULL,
    UserName NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Users_IsActive DEFAULT 1,

    IsBlocked BIT NOT NULL
        CONSTRAINT DF_Users_IsBlocked DEFAULT 0,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Users PRIMARY KEY (UserId)
);

CREATE TABLE Groups
(
    GroupId INT IDENTITY(1,1) NOT NULL,
    GroupName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(250) NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Groups_IsActive DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Groups_CreatedAt DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Groups PRIMARY KEY (GroupId),

    CONSTRAINT UQ_Groups_GroupName UNIQUE (GroupName)
);

CREATE TABLE UserGroups
(
    UserId NVARCHAR(50) NOT NULL,
    GroupId INT NOT NULL,

    CONSTRAINT PK_UserGroups
        PRIMARY KEY (UserId, GroupId),

    CONSTRAINT FK_UserGroups_Users
        FOREIGN KEY (UserId) REFERENCES Users(UserId),

    CONSTRAINT FK_UserGroups_Groups
        FOREIGN KEY (GroupId) REFERENCES Groups(GroupId)
);

CREATE TABLE Permissions
(
    PermissionId INT IDENTITY(1,1) NOT NULL,
    PermissionCode NVARCHAR(100) NOT NULL,
    PermissionName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Permissions_IsActive DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Permissions_CreatedAt
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Permissions
        PRIMARY KEY (PermissionId),

    CONSTRAINT UQ_Permissions_Code
        UNIQUE (PermissionCode)
);

CREATE TABLE GroupPermissions
(
    GroupId INT NOT NULL,
    PermissionId INT NOT NULL,

    CONSTRAINT PK_GroupPermissions
        PRIMARY KEY (GroupId, PermissionId),

    CONSTRAINT FK_GroupPermissions_Groups
        FOREIGN KEY (GroupId) REFERENCES Groups(GroupId),

    CONSTRAINT FK_GroupPermissions_Permissions
        FOREIGN KEY (PermissionId)
        REFERENCES Permissions(PermissionId)
);
