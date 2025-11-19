namespace UserManagementApi.Models;

public class User
{
    public int UserId { get; set; } 
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int RoleId { get; set; }
    public DateTime CreatedDate { get; set; }

    public Role Role { get; set; } = null!;
}
