
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UserManagementApi.Data;
using UserManagementApi.Dtos;
using UserManagementApi.Models;
namespace UserManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<PagedUsersResponse>> GetUsers([FromQuery] GetUsersRequest req)
    {
        var query = _db.Users
            .Include(u => u.Role)
                .ThenInclude(r => r.Permission)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.Search))
        {
            var search = req.Search.Trim();
            query = query.Where(u =>
                u.FirstName.Contains(search) ||
                u.LastName.Contains(search) ||
                u.Email.Contains(search) ||
                u.Username.Contains(search));
        }

        var totalCount = await query.CountAsync();

        var orderBy = (req.OrderBy ?? "createdDate").ToLowerInvariant();
        var dir = (req.OrderDirection ?? "asc").ToLowerInvariant();

        query = (orderBy, dir) switch
        {
            ("firstname", "desc") => query.OrderByDescending(u => u.FirstName),
            ("firstname", _)      => query.OrderBy(u => u.FirstName),

            ("lastname", "desc")  => query.OrderByDescending(u => u.LastName),
            ("lastname", _)       => query.OrderBy(u => u.LastName),

            ("email", "desc")     => query.OrderByDescending(u => u.Email),
            ("email", _)          => query.OrderBy(u => u.Email),

            ("username", "desc")  => query.OrderByDescending(u => u.Username),
            ("username", _)       => query.OrderBy(u => u.Username),

            ("createdate", "desc") => query.OrderByDescending(u => u.CreatedDate),
            ("createdate", _)      => query.OrderBy(u => u.CreatedDate),

            _ => query.OrderBy(u => u.CreatedDate)
        };

        var page = req.PageNumber <= 0 ? 1 : req.PageNumber;
        var pageSize = req.PageSize <= 0 ? 10 : req.PageSize;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var data = items.Select(u => new UserListItemDto
        {
            UserId = u.UserId.ToString(),
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Username = u.Username,
            Role = new RoleDto
            {
                RoleId = u.Role.RoleId.ToString(),
                RoleName = u.Role.RoleName
            },
            Permissions = new PermissionDto
            {
                PermissionId = u.Role.Permission.PermissionId.ToString(),
                PermissionName = u.Role.Permission.PermissionName
            },
            CreatedDate = u.CreatedDate
        }).ToList();

        var response = new PagedUsersResponse
        {
            DataSource = data,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return Ok(response);
    }
    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailResponse>> GetUserById(string id)
    {
        var response = new UserDetailResponse();

        if (!int.TryParse(id, out var userId))
        {
            response.Status = new StatusDto
            {
                Code = "400",
                Description = "Invalid user id format."
            };
            response.Data = null;
            return BadRequest(response);
        }

        var user = await _db.Users
            .Include(u => u.Role)
                .ThenInclude(r => r.Permission)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            response.Status = new StatusDto
            {
                Code = "404",
                Description = "User not found."
            };
            response.Data = null;
            return NotFound(response);
        }

        var dto = new UserDetailDto
        {
            Id = user.UserId.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Username = user.Username,
            Role = new RoleDto
            {
                RoleId = user.Role.RoleId.ToString(),
                RoleName = user.Role.RoleName
            },
            Permissions = new List<PermissionDto>
            {
                new PermissionDto
                {
                    PermissionId = user.Role.Permission.PermissionId.ToString(),
                    PermissionName = user.Role.Permission.PermissionName
                }
            }
        };

        response.Status = new StatusDto
        {
            Code = "200",
            Description = "Success"
        };
        response.Data = dto;

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> CreateUser([FromBody] CreateUserRequest req)
    {
        var response = new CreateUserResponse();

        if (string.IsNullOrWhiteSpace(req.FirstName) ||
            string.IsNullOrWhiteSpace(req.LastName) ||
            string.IsNullOrWhiteSpace(req.Email) ||
            string.IsNullOrWhiteSpace(req.RoleId) ||
            string.IsNullOrWhiteSpace(req.Username) ||
            string.IsNullOrWhiteSpace(req.Password) ||
            req.Permissions == null || req.Permissions.Count == 0)
        {
            response.Status = new StatusDto
            {
                Code = "400",
                Description = "Missing required fields."
            };
            response.Data = null;
            return BadRequest(response);
        }

        if (!int.TryParse(req.RoleId, out var roleId))
        {
            response.Status = new StatusDto
            {
                Code = "400",
                Description = "Invalid roleId format."
            };
            response.Data = null;
            return BadRequest(response);
        }

        var role = await _db.Roles
            .Include(r => r.Permission)
            .FirstOrDefaultAsync(r => r.RoleId == roleId);

        if (role == null)
        {
            response.Status = new StatusDto
            {
                Code = "404",
                Description = "Role not found."
            };
            response.Data = null;
            return NotFound(response);
        }

        var permInput = req.Permissions.First(); 
        if (!int.TryParse(permInput.PermissionId, out var permId))
        {
            response.Status = new StatusDto
            {
                Code = "400",
                Description = "Invalid permissionId format."
            };
            response.Data = null;
            return BadRequest(response);
        }

        var perm = await _db.Permissions
            .FirstOrDefaultAsync(p => p.PermissionId == permId && p.RoleId == roleId);

        if (perm == null)
        {

            perm = new Permission
            {
                PermissionName = role.RoleName, 
                RoleId = role.RoleId,
                IsReadable = permInput.IsReadable,
                IsWriteable = permInput.IsWriteable,
                IsDeleteable = permInput.IsDeleteable
            };
            _db.Permissions.Add(perm);
            await _db.SaveChangesAsync();
        }
        else
        {
            perm.IsReadable = permInput.IsReadable;
            perm.IsWriteable = permInput.IsWriteable;
            perm.IsDeleteable = permInput.IsDeleteable;
            await _db.SaveChangesAsync();
        }

        var user = new User
        {
            FirstName = req.FirstName.Trim(),
            LastName = req.LastName.Trim(),
            Email = req.Email.Trim(),
            Phone = req.Phone?.Trim() ?? "",
            Username = req.Username.Trim(),
            RoleId = role.RoleId,
            CreatedDate = DateTime.UtcNow 
        };


        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, req.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var result = new UserCreateResultDto
        {
            UserId = user.UserId.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Username = user.Username,
            Role = new RoleDto
            {
                RoleId = role.RoleId.ToString(),
                RoleName = role.RoleName
            },
            Permissions = new List<PermissionDto>
            {
                new PermissionDto
                {
                    PermissionId = perm.PermissionId.ToString(),
                    PermissionName = perm.PermissionName
                }
            }
        };

        response.Status = new StatusDto
        {
            Code = "201",
            Description = "User created successfully."
        };
        response.Data = result;

        return CreatedAtAction(nameof(GetUserById), new { id = user.UserId.ToString() }, response);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateUserResponse>> UpdateUser(string id, [FromBody] UpdateUserRequest req)
    {
        var response = new UpdateUserResponse();

        if (!int.TryParse(id, out var userId))
        {
            response.Status = new StatusDto { Code = "400", Description = "Invalid user id format" };
            return BadRequest(response);
        }

        var user = await _db.Users
            .Include(u => u.Role)
            .ThenInclude(r => r.Permission)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            response.Status = new StatusDto { Code = "404", Description = "User not found" };
            return NotFound(response);
        }

        if (!int.TryParse(req.RoleId, out var roleId))
        {
            response.Status = new StatusDto { Code = "400", Description = "Invalid roleId format" };
            return BadRequest(response);
        }

        var role = await _db.Roles
            .Include(r => r.Permission)
            .FirstOrDefaultAsync(r => r.RoleId == roleId);

        if (role == null)
        {
            response.Status = new StatusDto { Code = "404", Description = "Role not found" };
            return NotFound(response);
        }

        user.FirstName = req.FirstName.Trim();
        user.LastName = req.LastName.Trim();
        user.Email = req.Email.Trim();
        user.Phone = req.Phone?.Trim();
        user.Username = req.Username.Trim();
        user.RoleId = role.RoleId;

        if (!string.IsNullOrWhiteSpace(req.Password))
        {
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, req.Password);
        }

        var permInput = req.Permissions.First();
        if (!int.TryParse(permInput.PermissionId, out var permId))
        {
            response.Status = new StatusDto { Code = "400", Description = "Invalid permissionId format" };
            return BadRequest(response);
        }

        var perm = await _db.Permissions
            .FirstOrDefaultAsync(p => p.PermissionId == permId && p.RoleId == roleId);

        if (perm != null)
        {
            perm.IsReadable = permInput.IsReadable;
            perm.IsWriteable = permInput.IsWriteable;
            perm.IsDeleteable = permInput.IsDeleteable;
        }

        await _db.SaveChangesAsync();

        var result = new UserCreateResultDto
        {
            UserId = user.UserId.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Username = user.Username,
            Role = new RoleDto
            {
                RoleId = role.RoleId.ToString(),
                RoleName = role.RoleName
            },
            Permissions = new List<PermissionDto>
            {
                new PermissionDto
                {
                    PermissionId = perm.PermissionId.ToString(),
                    PermissionName = perm.PermissionName
                }
            }
        };

        response.Status = new StatusDto { Code = "200", Description = "User updated successfully" };
        response.Data = result;

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteUserResponse>> DeleteUser(string id)
    {
        var response = new DeleteUserResponse();

       
        if (!int.TryParse(id, out var userId))
        {
            response.Status = new StatusDto
            {
                Code = "400",
                Description = "Invalid user id format."
            };
            response.Data = new DeleteUserResultDto
            {
                Result = false,
                Message = "User id must be a valid number."
            };
            return BadRequest(response);
        }

        var user = await _db.Users.FindAsync(userId);

        if (user == null)
        {
            response.Status = new StatusDto
            {
                Code = "404",
                Description = "User not found."
            };
            response.Data = new DeleteUserResultDto
            {
                Result = false,
                Message = "User not found."
            };
            return NotFound(response);
        }

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        response.Status = new StatusDto
        {
            Code = "200",
            Description = "User deleted successfully."
        };
        response.Data = new DeleteUserResultDto
        {
            Result = true,
            Message = "User deleted successfully."
        };

        return Ok(response);
    }


}
