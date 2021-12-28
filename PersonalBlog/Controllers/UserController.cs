using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Models.Requests;
using PersonalBlog.Data.Responses;
using PersonalBlog.Services.Interfaces;
using System.Security.Claims;

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

        [HttpPost("createrole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateRole([FromBody] Role roleToCreate)
        {
            await _roleService.CreateRoleAsync(roleToCreate);
            return Ok(roleToCreate);
        }

        [HttpPost("attachrole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AttachRole([FromBody] AttachRoleRequest roleRequest)
        {
            var user = await _userService.GetUserByNameAsync(roleRequest.UserName);
            var role = await _roleService.GetRoleByNameAsync(roleRequest.RoleName);

            user.Role = role;
            user.Roles = role.Name;
            await _userService.UpdateUserAsync(user);

            return Ok();
        }

        [HttpGet("getusers")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("getuser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUser([FromBody] string userName)
        {
            var user = await _userService.GetUserByNameAsync(userName);
            return Ok(user);
        }

        [HttpGet("updateuserbyadmin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUserByAdmin([FromBody] User userToUpdate)
        {
            await _userService.UpdateUserAsync(userToUpdate);
            return Ok(userToUpdate);
        }

        [Authorize]
        [HttpGet("updateuserbyadmin")]
        public async Task<IActionResult> UpdateUser([FromBody] User userToUpdate)
        {
            if (!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };
            if(userToUpdate.Id != userId)
            {
                return BadRequest(new ErrorResponse("Only account owner or admin can edit account"));
            }
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
