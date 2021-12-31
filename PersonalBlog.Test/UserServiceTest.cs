using Microsoft.AspNetCore.Identity;
using Moq;
using PersonalBlog.Data.Exceptions;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services;
using PersonalBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PersonalBlog.Test
{
    public class UserServiceTest
    {
        private readonly UserService _serviceUnderTest;
        private readonly Mock<IGenericRepository<User>> _userRepoMock = new();
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock = new();
        private readonly Mock<IRoleService> _roleServiceMock = new();
        private readonly List<User> Users = new()
        {
            new User()
            {
                UserName = "user1",
                Email = "user1@test.com",
                Roles = "user"
            },
            new User()
            {
                UserName = "user2",
                Email = "user2@test.com",
                Roles = "user"
            },
            new User()
            {
                UserName = "admin",
                Email = "admin@test.com",
                Roles = "admin"
            }
        };

        public UserServiceTest()
        {
            _serviceUnderTest = new UserService(_userRepoMock.Object, _passwordHasherMock.Object, _roleServiceMock.Object);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnUser()
        {
            // Arrange
            var user = new User()
            {
                UserName = "testuser",
                Email = "testuser@test.com",
                Roles = "user"
            };
            _userRepoMock.Setup(option => option.CreateAsync(user)).Returns(Task.FromResult(user));

            // Act
            var actual = await _serviceUnderTest.CreateUserAsync(user);

            // Assert
            Assert.Equal(user, actual);
        }
        [Fact]
        public async Task RegisterUser_ShouldReturnUser()
        {
            // Arrange
            string password = "1q2w3e";
            var role = new Role()
            {
                Name = "user"
            };
            var user = new User()
            {
                UserName = "testuser1",
                Email = "testuser1@test.com",
                Role = role,
                Roles = role.Name,
            };
            var hashMoq = _passwordHasherMock.Setup(hash => hash.HashPassword(user, password));
            _roleServiceMock.Setup(option => option.GetRoleByNameAsync(role.Name)).ReturnsAsync(role);
            _userRepoMock.Setup(option => option.CreateAsync(user)).Returns(Task.CompletedTask);

            // Act
            var actual = await _serviceUnderTest.RegisterUserAsync(user, password);

            // Assert
            Assert.Equal(user.UserName, actual.UserName);
            Assert.Equal(user.Email, actual.Email);
            Assert.Equal(user.Role, actual.Role);
            Assert.Equal(user.Roles, actual.Roles);
        }
        [Fact]
        public async Task RegisterUser_ShouldReturnEmailExistException()
        {
            // Arrange
            string password = "1q2w3e";
            var user = new User()
            {
                Email = "user2@test.com"
            };
            _userRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Users);

            // Assert
            var ex = Assert.ThrowsAsync<EmailAlreadyUsedException>(async () => await _serviceUnderTest.RegisterUserAsync(user, password));
            Assert.Equal(ex.Result.Message, $"Email is already in use! {user.Email}");
        }
        [Fact]
        public async Task RegisterUser_ShouldReturnUserNameAllreadyExist()
        {
            // Arrange
            string password = "1q2w3e";
            var user = new User()
            {
                UserName = "user1",
            };
            _userRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Users);

            // Assert
            var ex = Assert.ThrowsAsync<UserNameAlreadyUsedException>(async () => await _serviceUnderTest.RegisterUserAsync(user, password));
            Assert.Equal(ex.Result.Message, $"User name already in use! {user.UserName}");
        }
        [Fact]
        public async Task GetUserByName_GetUserByMail_ShouldReturnUser()
        {
            // Arrange
            string userName = "user1";
            string email = "user2@test.com";
            _userRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Users);

            // Act
            var userByUserName = await _serviceUnderTest.GetUserByNameAsync(userName);
            var userByEmail = await _serviceUnderTest.GetUserByEmailAsync(email);

            // Assert
            Assert.True(Users.Exists(u => u == userByUserName));
            Assert.True(Users.Exists(u => u == userByEmail));
        }
        [Fact]
        public async Task GetUserById_ReturnException()
        {
            // Arrange
            var userId = new Guid();
            _userRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Users);

            // Assert
            var ex = Assert.ThrowsAsync<NullableUserException>(async () => await _serviceUnderTest.GetUserByIdAsync(userId));
            Assert.Equal("User is null!", ex.Result.Message);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange 
            _userRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Users);

            // Act
            var users = await _serviceUnderTest.GetAllUsersAsync();

            // Assert
            Assert.Equal(Users, users);
        }
        
    }
}
