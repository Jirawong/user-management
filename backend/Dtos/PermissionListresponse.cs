namespace UserManagementApi.Dtos;

public class PermissionsListResponse
{
    public StatusDto Status { get; set; } = new();
    public List<PermissionDto> Data { get; set; } = new();
}
