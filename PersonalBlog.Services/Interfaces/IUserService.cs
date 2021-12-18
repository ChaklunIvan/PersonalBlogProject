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
        PasswordVerificationResult VerifyUserPassword(User userToVerify, string password);

    }
}
