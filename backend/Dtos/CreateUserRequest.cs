namespace UserManagementApi.Dtos;

public class CreateUserPermissionInput
{
    public string PermissionId { get; set; } = null!;
    public bool IsReadable { get; set; }
    public bool IsWriteable { get; set; }
    public bool IsDeleteable { get; set; }
}

public class CreateUserRequest
{
    public string? Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName  { get; set; } = null!;
    public string Email     { get; set; } = null!;
    public string? Phone    { get; set; }
    public string RoleId    { get; set; } = null!;
    public string Username  { get; set; } = null!;
    public string Password  { get; set; } = null!;
    public List<CreateUserPermissionInput> Permissions { get; set; } = new();
}
