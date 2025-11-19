namespace UserManagementApi.Dtos;

public class GetUsersRequest
{
    public string? OrderBy { get; set; } = "createDate"; 
    public string? OrderDirection { get; set; } = "asc"; 
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
}
