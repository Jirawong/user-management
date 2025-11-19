using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementApi.Data;
using UserManagementApi.Dtos;

namespace UserManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _db;

    public RolesController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/roles
    [HttpGet]
    public async Task<ActionResult<RolesListResponse>> GetAllRoles()
    {
        var response = new RolesListResponse();

        var roles = await _db.Roles.ToListAsync();

        response.Data = roles
            .Select(r => new RoleDto
            {
                RoleId = r.RoleId.ToString(),
                RoleName = r.RoleName
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
