using Moq;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services;
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
    }
}
