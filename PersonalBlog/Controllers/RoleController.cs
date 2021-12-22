using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost("createrole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateRole([FromBody] Role roleToCreate)
        {
            await _roleService.CreateRoleAsync(roleToCreate);
            return Ok(roleToCreate);
        }
    }
}
