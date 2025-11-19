namespace UserManagementApi.Dtos;

public class RolesListResponse
{
    public StatusDto Status { get; set; } = new();
    public List<RoleDto> Data { get; set; } = new();
}
