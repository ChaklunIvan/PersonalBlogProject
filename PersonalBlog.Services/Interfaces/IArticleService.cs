using PersonalBlog.Data.Models;

namespace PersonalBlog.Services.Interfaces
{
    public interface IArticleService
    {
        Task<Article> CreateArticleAsync(Article articleToCreate);
        Task<Article> UpdateArticleAsync(Article articleToUpdate);
        Task DeleteArticleAsync(Guid articleId);
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(Guid articleId);
        Task<Article> GetArticleByTagAsync(Tag tag);
        Task<IEnumerable<Article>> GetArticlesByBlogNameAsync(string blogName);
        Task<Article> GetArticleByNameAsync(string articleName);
    }
}
