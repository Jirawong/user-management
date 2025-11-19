namespace UserManagementApi.Dtos;

public class PagedUsersResponse
{
    public List<UserListItemDto> DataSource { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
