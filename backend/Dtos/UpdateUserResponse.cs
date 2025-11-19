namespace UserManagementApi.Dtos;

public class UpdateUserResponse
{
    public StatusDto Status { get; set; } = new();
    public UserCreateResultDto? Data { get; set; }
}
