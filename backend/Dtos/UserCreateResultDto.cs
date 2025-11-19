namespace UserManagementApi.Dtos;

public class UserCreateResultDto
{
    public string UserId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public RoleDto Role { get; set; } = null!;
    public string Username { get; set; } = null!;
    public List<PermissionDto> Permissions { get; set; } = new();
}

public class CreateUserResponse
{
    public StatusDto Status { get; set; } = new();
    public UserCreateResultDto? Data { get; set; }
}
