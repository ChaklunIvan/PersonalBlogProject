using Moq;
using PersonalBlog.Data.Exceptions;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PersonalBlog.Test
{
    public class BlogServiceTest
    {
        private readonly BlogService _serviceUnderTest;
        private readonly Mock<IGenericRepository<Blog>> _blogRepoMock = new();
        private readonly List<Blog> Blogs = new()
        {
            new Blog()
            {
                Title = "Blog1",
            },
            new Blog()
            {
                Title = "Blog2",
            }
        };
        public BlogServiceTest()
        {
            _serviceUnderTest = new BlogService(_blogRepoMock.Object);
        }

        [Fact]
        public async Task CreateBlog_ShouldReturnBlog()
        {
            // Arrange 
            var blog = new Blog()
            {
                Title = "Blog3"
            };
            _blogRepoMock.Setup(option => option.CreateAsync(blog)).Returns(Task.FromResult(blog));

            // Act
            var actual = await _serviceUnderTest.CreateBlogAsync(blog);

            // Assert
            Assert.Equal(blog.Title, actual.Title);
        }
        [Fact]
        public async Task GetAllBlogs_ShouldReturnAllBlogs()
        {
            // Arrange
            _blogRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Blogs);

            // Act
            var blogs = await _serviceUnderTest.GetAllBlogAsync();

            // Assert
            Assert.Equal(Blogs, blogs);
        }
        [Fact]
        public async Task GetBlogByName_ShouldReturnBlog()
        {
            // Arrange
            string blogName = "Blog1";
            _blogRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Blogs);

            // Act
            var blog = await _serviceUnderTest.GetBlogByNameAsync(blogName);

            // Assert
            Assert.True(Blogs.Exists(b => b == blog));
        }
        [Fact]
        public async Task GetBlogByUser_ShouldReturnBlog()
        {
            // Arrange
            var user = new User()
            {
                UserName = "testuser",
                Email = "test@test.com"
            };
            Blogs[0].User = user;
            Blogs.Remove(Blogs[1]);
            IEnumerable<Blog> expectedBlogs = Blogs;
            _blogRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Blogs);

            // Act
            var actualBlogs = await _serviceUnderTest.GetBlogsByUser(user);

            // Assert
            Assert.Equal(actualBlogs, expectedBlogs);
        }
        [Fact]
        public async Task UpdateBlog_ShouldReturnBlog()
        {
            // Arrange
            var blog = Blogs[0];
            blog.Title = "Test title";
            _blogRepoMock.Setup(option => option.GetByIdAsync(blog.Id)).ReturnsAsync(blog);
            _blogRepoMock.Setup(option => option.UpdateAsync(blog)).Returns(Task.CompletedTask);

            // Act
            var result = await _serviceUnderTest.UpdateBlogAsync(blog);
            
            // Assert
            Assert.Equal(Blogs[0], result);
        }
        [Fact]
        public void UpdateBlog_ShouldReturnNull_NullabaleException()
        {
            // Arrange
            var blogId = new Guid();
            var blog = Blogs[0];
            _blogRepoMock.Setup(option => option.GetByIdAsync(blogId)).ReturnsAsync(() => null);

            // Assert
            var ex = Assert.ThrowsAsync<NullableBlogException>(async () => await _serviceUnderTest.UpdateBlogAsync(blog));
            Assert.Equal("blog is null", ex.Result.Message);
        }
    }
}
