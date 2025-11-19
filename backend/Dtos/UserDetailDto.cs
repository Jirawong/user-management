namespace UserManagementApi.Dtos;

public class UserDetailDto
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public RoleDto Role { get; set; } = null!;
    public string Username { get; set; } = null!;
    public List<PermissionDto> Permissions { get; set; } = new();
}
