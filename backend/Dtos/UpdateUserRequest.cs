namespace UserManagementApi.Dtos;

public class UpdateUserPermissionInput
{
    public string PermissionId { get; set; } = null!;
    public bool IsReadable { get; set; }
    public bool IsWriteable { get; set; }
    public bool IsDeleteable { get; set; }
}

public class UpdateUserRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName  { get; set; } = null!;
    public string Email     { get; set; } = null!;
    public string? Phone    { get; set; }
    public string RoleId    { get; set; } = null!;
    public string Username  { get; set; } = null!;
    public string Password  { get; set; } = null!;
    public List<UpdateUserPermissionInput> Permissions { get; set; } = new();
}
