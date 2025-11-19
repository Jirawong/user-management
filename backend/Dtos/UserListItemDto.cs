namespace UserManagementApi.Dtos;

public class UserListItemDto
{
    public string UserId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public RoleDto Role { get; set; } = null!;
    public string Username { get; set; } = null!;
    public PermissionDto Permissions { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
}
