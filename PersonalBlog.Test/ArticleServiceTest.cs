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
    public class ArticleServiceTest
    {
        private readonly ArticleService _serviceUnderTest;
        private readonly Mock<IGenericRepository<Article>> _articleMockRepo = new();
        private readonly List<Article> Articles = new()
        {
            new Article()
            {
                BlogTitle = "Blog1",
                Text = "Some text",
                Title = "Article1",
                Tags = new List<string>() { "Tag1", "Tag2"}
            },
            new Article()
            {
                BlogTitle = "Blog2",
                Text = "Some text",
                Title = "Article2",
                Tags= new List<string>() { "Tag2"}
            }
        };
        public ArticleServiceTest()
        {
            _serviceUnderTest = new ArticleService(_articleMockRepo.Object);
        }
        [Fact]
        public async Task CreateArticle_ShouldReturnAdticle()
        {
            // Arrange
            var article = new Article()
            { Title = "Test"};
            _articleMockRepo.Setup(option => option.CreateAsync(article)).Returns(Task.CompletedTask);

            // Act
            var result = await _serviceUnderTest.CreateArticleAsync(article);

            // Assert
            Assert.Equal(article.Title, result.Title);
        }
        [Fact]
        public async Task GetAllArticles_ShouldReturnArticle()
        {
            // Arrange
            _articleMockRepo.Setup(option => option.GetAllAsync()).ReturnsAsync(Articles);

            // Act 
            var actual = await _serviceUnderTest.GetAllArticlesAsync();

            // Assert
            Assert.Equal(Articles, actual);
        }
        [Fact]
        public void GetArticleById_ShouldReturnException()
        {
            // Arrange
            var articleId = new Guid();
            _articleMockRepo.Setup(option => option.GetByIdAsync(articleId)).ReturnsAsync(() => null);

            // Assert
            var ex = Assert.ThrowsAsync<NullableArticleException>(async () => await _serviceUnderTest.GetArticleByIdAsync(articleId));
            Assert.Equal("Article is null", ex.Result.Message);
        }
        [Fact]
        public async Task GetArticleByBlogName_ShouldReturnArticle()
        {
            // Arrange
            string blogName = "Blog1";
            _articleMockRepo.Setup(option => option.GetAllAsync()).ReturnsAsync(Articles);

            // Act
            var actual = await _serviceUnderTest.GetArticlesByBlogNameAsync(blogName);

            // Assert
            Assert.Equal(Articles[0],actual.FirstOrDefault(a => a.BlogTitle == blogName));
        }
        [Fact]
        public async Task GetArticleByTag_ShouldReturnArticle()
        {
            // Arrange
            var tag = "Tag1";
            _articleMockRepo.Setup(option => option.GetAllAsync()).ReturnsAsync(Articles);

            // Act
            var actual = await _serviceUnderTest.GetArticleByTagAsync(tag);

            // Assert
            Assert.Equal(Articles[0], actual);
        }
        [Fact]
        public async Task GetArticleByName_ShouldReturnArticle()
        {
            // Arrange
            var articleName = "Article1";
            _articleMockRepo.Setup(option => option.GetAllAsync()).ReturnsAsync(Articles);

            // Act
            var actual = await _serviceUnderTest.GetArticleByNameAsync(articleName);

            // Assert
            Assert.Equal(Articles[0], actual);
        }
        [Fact]
        public async Task UpdateArticle_ShouldeReturnArticle()
        {
            // Arrange
            var article = Articles[0];
            article.Text = "123";
            article.Title = "Test";
            _articleMockRepo.Setup(option => option.GetByIdAsync(article.Id)).ReturnsAsync(article);
            _articleMockRepo.Setup(option => option.UpdateAsync(article)).Returns(Task.CompletedTask);

            // Act
            var actual = await _serviceUnderTest.UpdateArticleAsync(article);

            // Assert
            Assert.Equal(Articles[0], actual);
        }
        [Fact]
        public void UpdateArticle_ShuoldReturnNull_NullableExpection()
        {
            // Arrange
            var articleId = new Guid();
            var article = Articles[0];
            _articleMockRepo.Setup(option => option.GetByIdAsync(articleId)).ReturnsAsync(() => null);

            // Assert
            var ex = Assert.ThrowsAsync<NullableArticleException>(async () => await _serviceUnderTest.UpdateArticleAsync(article));
            Assert.Equal("Article is null", ex.Result.Message);
        }
    }
}
