namespace UserManagementApi.Dtos;

public class UserDetailResponse
{
    public StatusDto Status { get; set; } = new();
    public UserDetailDto? Data { get; set; }
}
