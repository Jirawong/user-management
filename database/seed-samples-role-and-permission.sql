
-- seed roles
INSERT INTO Roles (RoleId, RoleName)
VALUES
    (1, 'Super Admin'),
    (2, 'Admin'),
    (3, 'HR Admin'),
    (4, 'Employee');

-- seed permissions
INSERT INTO Permissions (PermissionId, PermissionName, IsReadable, IsWriteable, IsDeleteable, RoleId)
VALUES
    (1, 'Super Admin', 1, 1, 1, 1),
    (2, 'Admin',        1, 1, 0, 2),
    (4, 'Employee',     1, 1, 0, 4),
    (7, 'HR Admin',     1, 0, 0, 3);
