using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementApi.Data;
using UserManagementApi.Dtos;

namespace UserManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PermissionsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<PermissionsListResponse>> GetAllPermissions()
    {
        var response = new PermissionsListResponse();

        var items = await _db.Permissions.ToListAsync();

        response.Data = items
            .Select(p => new PermissionDto
            {
                PermissionId = p.PermissionId.ToString(),
                PermissionName = p.PermissionName
            })
            .ToList();

        response.Status = new StatusDto
        {
            Code = "200",
            Description = "Success"
        };

        return Ok(response);
    }
}
