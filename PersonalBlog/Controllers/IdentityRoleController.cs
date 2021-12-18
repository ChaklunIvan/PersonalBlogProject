using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class IdentityRoleController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGenericRepository<Role> _roleRepository;

        public IdentityRoleController(IUserService userService, IGenericRepository<Role> roleRepository)
        {
            _userService = userService;
            _roleRepository = roleRepository;
        }
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole(Role roleToCreate) 
        {
            await _roleRepository.CreateAsync(roleToCreate);
            return Ok(_roleRepository.GetByIdAsync(roleToCreate.Id));
        }
        [HttpPost("assignrole")]
        public async Task<IActionResult> AssignRole(Role roleToAssign, string userName)
        {
            var user = await _userService.GetUserByNameAsync(userName);
            user.RoleName = roleToAssign.Name;
            user.Role = roleToAssign;
            return Ok(user);
        }
    }
}
