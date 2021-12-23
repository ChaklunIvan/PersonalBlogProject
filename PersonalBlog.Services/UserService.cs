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

            var role = await _roleService.GetRoleByNameAsync("user");
            userToRegister.Role = role;
            userToRegister.Roles = role.Name;
            userToRegister.PasswordHash = _passwordHasher.HashPassword(null, password);
            return await CreateUserAsync(userToRegister);
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            var users = await GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.UserName == userName);
            
            return user;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var users = await GetAllUsersAsync();
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
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }


        public async Task<User> UpdateUserAsync(User userToUpdate)
        {
            var user = await GetUserByIdAsync(userToUpdate.Id);
            user.UserName = userToUpdate.UserName;
            user.Email = userToUpdate.Email;
            await _userRepository.UpdateAsync(user);

            return user;
        }

        public async Task DeleteUserAsync(string userName)
        {
            var user = await GetUserByNameAsync(userName);
            await _userRepository.DeleteAsync(user);
        }

        public Task<PasswordVerificationResult> VerifyUserPassword(User userToVerify, string password)
        {
            var verifyResult = _passwordHasher.VerifyHashedPassword(userToVerify, userToVerify.PasswordHash, password);
            return Task.FromResult(verifyResult);
        }

    }

}
