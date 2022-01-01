using Moq;
using PersonalBlog.Data.Exceptions;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PersonalBlog.Test
{
    public class RefreshTokenServiceTest
    {
        private readonly RefreshTokenService _serviceUnderTest;
        private readonly Mock<IGenericRepository<RefreshToken>> _refreshMockRepo = new();
        private readonly List<RefreshToken> _tokenList = new()
        {
            new RefreshToken()
            {
                Token = "Token"
            }
        };

        public RefreshTokenServiceTest()
        {
            _serviceUnderTest = new RefreshTokenService(_refreshMockRepo.Object);
        }
        [Fact]
        public async Task CreateToken_ShouldReturnToken()
        {
            // Arrange
            var token = new RefreshToken()
            {
                Token = "Test"
            };
            _refreshMockRepo.Setup(option => option.CreateAsync(token)).Returns(Task.CompletedTask);

            // Act
            var result = await _serviceUnderTest.CreateTokenAsync(token);

            // Assert
            Assert.Equal(token,result);
        }

        [Fact]
        public async Task GetByToken_ShouldReturnToken()
        {
            // Arrange
            string token = "Token";
            _refreshMockRepo.Setup(option => option.GetAllAsync()).ReturnsAsync(_tokenList);

            // Act
            var actual = await _serviceUnderTest.GetByTokenAsync(token);

            // Assert
            Assert.True(_tokenList.Exists(t => t == actual));
        }
        [Fact]
        public void GetByToken_ShouldReturnNull_NullableException()
        {
            // Arrange
            string token = " ";
            _refreshMockRepo.Setup(option => option.GetAllAsync()).ReturnsAsync(_tokenList);

            // Assert
            var ex = Assert.ThrowsAsync<NullableTokenException>(async () => await _serviceUnderTest.GetByTokenAsync(token));
            Assert.Equal("There is no tokens!", ex.Result.Message);
        }
    }
}
