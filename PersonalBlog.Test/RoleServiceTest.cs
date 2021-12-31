using Moq;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PersonalBlog.Test
{
    public class RoleServiceTest
    {
        private readonly RoleService _serviceUnderTest;
        private readonly Mock<IGenericRepository<Role>> _roleRepoMock = new();
        private readonly List<Role> roles = new()
            {
                new Role() { Name = "user" },
                new Role() { Name = "admin"}
            };
        public RoleServiceTest()
        {
            _serviceUnderTest = new RoleService(_roleRepoMock.Object);
        }

        [Fact]
        public async Task GetAllRoles_ShouldReturn_AllRoles_OrNull()
        {
            // Arrange
            
            _roleRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(roles);

            // Act
            var actualResult = await _serviceUnderTest.GetAllRolesAsync();

            // Assert
            Assert.Equal(roles, actualResult);

        }
        [Fact]
        public async Task GetRoleByName_ShouldReturnRole_WhenRoleExists()
        {
            // Arrange
            _roleRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(roles);

            // Act
            var userRole = await _serviceUnderTest.GetRoleByNameAsync("user");
            var adminRole = await _serviceUnderTest.GetRoleByNameAsync("admin");

            // Assert
            Assert.True(roles.Exists(r => r == userRole));
            Assert.True(roles.Exists(r => r == adminRole));
        }
        [Fact]
        public async Task CreateRole_ShouldReturnRole()
        {
            // Arrange
            var role  = new Role()
            {
                Name = "moderator"
            };
            _roleRepoMock.Setup(option => option.CreateAsync(role)).Returns(Task.CompletedTask);

            // Act
            var actual =  await _serviceUnderTest.CreateRoleAsync(role);

            // Assert
            Assert.Equal(role, actual);
        }

    }
}