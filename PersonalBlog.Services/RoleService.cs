using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;

        public RoleService(IGenericRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }


        public async Task<Role> CreateRoleAsync(Role roleToCreate)
        {
            await _roleRepository.CreateAsync(roleToCreate);
            return roleToCreate;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            var roles = await _roleRepository.GetAllAsync();
            var role = roles.FirstOrDefault(r => r.Name == roleName);
            return role;
        }
        
    }
}
