using PersonalBlog.Data.Models;

namespace PersonalBlog.Services.Interfaces
{
    public interface IArticleService
    {
        Task<Article> CreateArticleAsync(Article articleToCreate);
        Task<Article> UpdateArticleAsync(Article articleToUpdate);
        Task DeleteArticleAsync(Guid idarticleId);
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(Guid articleId);
        Task<Article> GetArticleByTag(Tag tag);
        Task<Article> GetArticleByTextAsync(string text);
    }
}
