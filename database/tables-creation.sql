-- ROLES
CREATE TABLE Roles (
    RoleId      INT IDENTITY(1,1) PRIMARY KEY,
    RoleName    NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- USERS
CREATE TABLE Users (
    UserId       INT IDENTITY(1,1) PRIMARY KEY,
    FirstName    NVARCHAR(100) NOT NULL,
    LastName     NVARCHAR(100) NOT NULL,
    Email        NVARCHAR(255) NOT NULL UNIQUE,
    Phone        NVARCHAR(50) NULL,
    Username     NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    RoleId       INT NOT NULL,

    CONSTRAINT FK_Users_Roles
        FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);
GO

-- PERMISSIONS
CREATE TABLE Permissions (
    PermissionId   INT IDENTITY(1,1) PRIMARY KEY,
    PermissionName NVARCHAR(100) NOT NULL,
    IsReadable     BIT NOT NULL DEFAULT 0,
    IsWriteable    BIT NOT NULL DEFAULT 0,
    IsDeleteable   BIT NOT NULL DEFAULT 0,
    RoleId         INT NOT NULL,

    CONSTRAINT FK_Permissions_Roles
        FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);
GO
