
using PersonalBlog.Data.Models;

namespace PersonalBlog.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<Role> CreateRoleAsync(Role roleToCreate);
    }
}
