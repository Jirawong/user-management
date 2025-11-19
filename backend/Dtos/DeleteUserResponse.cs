namespace UserManagementApi.Dtos;

public class DeleteUserResponse
{
    public StatusDto Status { get; set; } = new();
    public DeleteUserResultDto Data { get; set; } = new();
}
