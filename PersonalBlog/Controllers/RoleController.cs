using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Models.Requests;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public RoleController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
        [HttpPost("createrole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateRole([FromBody] Role roleToCreate)
        {
            await _roleService.CreateRoleAsync(roleToCreate);
            return Ok(roleToCreate);
        }
        [HttpPost("assignrole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AssignAdminRole([FromBody] string userName)
        {
            await _userService.AttachRoleToUserAsync(userName);
            return Ok();
        }
    }
}
