namespace UserManagementApi.Models;

public class Permission
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = null!;
    public bool IsReadable { get; set; }
    public bool IsWriteable { get; set; }
    public bool IsDeleteable { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
