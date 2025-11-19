namespace UserManagementApi.Models;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;

    public Permission Permission { get; set; } = null!;
    public ICollection<User> Users { get; set; } = new List<User>();
}
