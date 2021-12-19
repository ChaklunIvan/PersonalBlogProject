using Microsoft.AspNetCore.Identity;
using PersonalBlog.Data.Exceptions;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRoleService _roleService;


        public UserService(IGenericRepository<User> userRepository, IPasswordHasher<User> passwordHasher,
            IRoleService roleService)

        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleService = roleService;
        }

        public async Task<User> CreateUserAsync(User userToCreate)
        {
            await _userRepository.CreateAsync(userToCreate);
            return userToCreate;
        }

        public async Task<User> RegisterUserAsync(User userToRegister, string password)
        {
            var existingUserByEmail = await GetUserByEmailAsync(userToRegister.Email);
            if (existingUserByEmail != null)
            {
                throw new EmailAlreadyUsedException(existingUserByEmail.Email);
            }
            var existingUserByUserName = await GetUserByNameAsync(userToRegister.UserName);
            if (existingUserByUserName != null)
            {
                throw new UserNameAlreadyUsedException(existingUserByUserName.UserName);
            }

            userToRegister.PasswordHash = _passwordHasher.HashPassword(null, password);
            return await CreateUserAsync(userToRegister);
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.UserName == userName);
            return user;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NullableUserException();
            }
            return user;
        }

        public Task<PasswordVerificationResult> VerifyUserPassword(User userToVerify, string password)
        {
            var verifyResult = _passwordHasher.VerifyHashedPassword(userToVerify, userToVerify.PasswordHash, password);
            return Task.FromResult(verifyResult);
        }

        public async Task<User> AttachRoleToUserAsync(string userName)
        {
            var user = await GetUserByNameAsync(userName);
            var roles = await _roleService.GetAllRolesAsync();
            var adminRole = roles.FirstOrDefault(r => r.Name == "admin");
            var userRole = roles.FirstOrDefault(r => r.Name == "user");
            if (user.Role == null)
            {
                user.Role = userRole;
                user.RoleName = userRole.Name;
            }
            else
            {
                user.Role = adminRole;
                user.RoleName = adminRole.Name;
            }
            await _userRepository.UpdateAsync(user);
            return user;
        }
    }

}
