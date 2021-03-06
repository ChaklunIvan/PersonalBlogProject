using Microsoft.AspNetCore.Identity;
using PersonalBlog.Data.Models;

namespace PersonalBlog.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User userToCreate);
        Task<User> RegisterUserAsync(User userToRegister, string password);
        Task<User> GetUserByNameAsync(string userName);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(User userToUpdate);
        Task DeleteUserAsync(string userName);
        Task<PasswordVerificationResult> VerifyUserPassword(User userToVerify, string password);
    }
}
