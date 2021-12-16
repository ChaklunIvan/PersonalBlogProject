using Microsoft.AspNetCore.Identity;
using PersonalBlog.Domain.Exceptions;
using PersonalBlog.Domain.Models;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(UserManager<User> userManager, IPasswordHasher<User> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> CreateUserAsync(User userToCreate)
        {
            var user = new User()
            {
                UserName = userToCreate.UserName,
                Email = userToCreate.Email,
                PasswordHash = userToCreate.PasswordHash
            };
            await _userManager.CreateAsync(user);
            return user;
        }

        public async Task<User> RegisterUserAsync(User userToRegister, string password)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(userToRegister.Email);
            if (existingUserByEmail != null)
            {
                throw new EmailAlreadyUsedException();
            }
            var existingUserByUserName = await _userManager.FindByNameAsync(userToRegister.UserName);
            if (existingUserByUserName != null)
            {
                throw new UserNameAlreadyUsedException();
            }

            userToRegister.PasswordHash = _passwordHasher.HashPassword(null, password);
            return await CreateUserAsync(userToRegister);
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                throw new ArgumentException("User in null");
            }
            return user;
        }

        public PasswordVerificationResult VerifyUserPassword(User userToVerify, string password)
        {
            return _passwordHasher.VerifyHashedPassword(userToVerify, userToVerify.PasswordHash, password);
        }

    }
        //public async Task DeleteUserAsync(string userName)
        //{
        //    var user = await _userManager.FindByNameAsync(userName);
        //    if (user != null)
        //    {
        //        await _userManager.DeleteAsync(user);
        //    }
        //}

        //public async Task<User> UpdateUserAsync(User userToUpdate)
        //{
        //    var user = await _userManager.FindByIdAsync(userToUpdate.Id);
        //    user.UserName = userToUpdate.UserName;
        //    user.Email = userToUpdate.Email;
        //    await _userManager.UpdateAsync(user);
        //    return user;
        //}
}
