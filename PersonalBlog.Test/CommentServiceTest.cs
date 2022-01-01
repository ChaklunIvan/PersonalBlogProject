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
    public class CommentServiceTest
    {
        private readonly CommentService _serviceUnderTest;
        private readonly Mock<IGenericRepository<Comment>> _commentRepoMock = new();
        private readonly List<Comment> Comments = new()
        {
            new Comment()
            {
                Value = "Comment"
            }
        };
        public CommentServiceTest()
        {
            _serviceUnderTest = new CommentService(_commentRepoMock.Object);
        }

        [Fact]
        public async Task CreateComment_ShouldReturnComment()
        {
            // Arrange 
            var comment = new Comment()
            {
                Value = "Test"
            };
            _commentRepoMock.Setup(option => option.CreateAsync(comment)).Returns(Task.FromResult(comment));

            // Act
            var actual = await _serviceUnderTest.CreateCommentAsync(comment);

            // Assert
            Assert.Equal(actual.Value, comment.Value);
        }
        [Fact]
        public async Task GetCommentsByArticle_ShouldReturnComments()
        {
            // Arrange
            var article = new Article()
            {
                Comments = new List<Comment>()
                {
                    Comments[0]
                },
                Title = "Test",
            };
            Comments[0].Article = article;
            _commentRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Comments);

            // Act
            var actual = await _serviceUnderTest.GetCommentsByArticleAsync(article.Id);

            // Assert
            Assert.Contains(Comments[0], actual);
        }
        [Fact]
        public async Task GetCommentsByUser_ShouldReturnComment()
        {
            // Arrange
            var user = new User()
            {
                UserName = "Test",
                Email = "Test",
            };
            Comments[0].User = user;
            _commentRepoMock.Setup(option => option.GetAllAsync()).ReturnsAsync(Comments);

            // Act
            var actual = await _serviceUnderTest.GetCommentsByUserAsync(user.Id);

            // Assert
            Assert.Contains(Comments[0], actual);
        }
        [Fact]
        public async Task UpdateComment_ShouldReturnComment()
        {
            // Arrange
            var comment = Comments[0];
            comment.Value = "comment";
            _commentRepoMock.Setup(option => option.GetByIdAsync(comment.Id)).ReturnsAsync(comment);
            _commentRepoMock.Setup(option => option.UpdateAsync(comment)).Returns(Task.CompletedTask);

            // Act
            var actual = await _serviceUnderTest.UpdateCommentAsync(comment);

            // Assert
            Assert.Equal(Comments[0], actual);
        }
        [Fact]
        public void UpdateComment_ShouldReturnNull_NullableException()
        {
            // Arrange
            var commentId = new Guid();
            var comment = Comments[0];
            _commentRepoMock.Setup(option => option.GetByIdAsync(commentId)).ReturnsAsync(() => null);

            // Assert
            var ex = Assert.ThrowsAsync<NullableCommentException>(async () => await _serviceUnderTest.UpdateCommentAsync(comment));
            Assert.Equal("Comment is null", ex.Result.Message);
        }
    }
}
