using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Models.Requests;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public UserController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole([FromBody] Role roleToCreate)
        {
            await _roleService.CreateRoleAsync(roleToCreate);
            return Ok(roleToCreate);
        }

        [HttpPost("attachrole")]
        public async Task<IActionResult> AttachRole([FromBody] AttachRoleRequest roleRequest)
        {
            var user = await _userService.GetUserByNameAsync(roleRequest.userName);
            var role = await _roleService.GetRoleByNameAsync(roleRequest.roleName);

            user.Role = role;
            user.Roles = role.Name;
            await _userService.UpdateUserAsync(user);

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getusers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser([FromBody] string userName)
        {
            var user = await _userService.GetUserByNameAsync(userName);
            return Ok(user);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] User userToUpdate)
        {
            await _userService.UpdateUserAsync(userToUpdate);
            return Ok(userToUpdate);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUser([FromBody] User userToDelete)
        {
            await _userService.DeleteUserAsync(userToDelete.UserName);
            return Ok();
        }
    }
}
