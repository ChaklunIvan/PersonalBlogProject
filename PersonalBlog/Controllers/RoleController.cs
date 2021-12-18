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
        public async Task<IActionResult> CreateRole([FromBody]Role roleToCreate) 
        {
            await _roleService.CreateRoleAsync(roleToCreate);
            return Ok(roleToCreate);
        }
        [HttpPost("assignrole")]
        public async Task<IActionResult> AssignRole([FromBody]RoleRequest roleRequest)
        {
            var role = await _roleService.GetRoleByNameAsync(roleRequest.RoleName);
            var user = await _userService.GetUserByNameAsync(roleRequest.UserName);
            await _userService.AttachRoleToUserAsync(role, user);
            return Ok();
        }
    }
}
