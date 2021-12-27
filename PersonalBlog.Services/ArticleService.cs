using PersonalBlog.Data.Exceptions;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IGenericRepository<Article> _articleRepository;

        public ArticleService(IGenericRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<Article> CreateArticleAsync(Article articleToCreate)
        {
            var article = new Article()
            {
                Title = articleToCreate.Title,
                Text = articleToCreate.Text,
                BlogTitle = articleToCreate.BlogTitle,
                Blog = articleToCreate.Blog,
                Comments = articleToCreate.Comments,
            };
            await _articleRepository.CreateAsync(article);
            return article;
        }

        public async Task DeleteArticleAsync(Guid articleId)
        {
            var article = await _articleRepository.GetByIdAsync(articleId);
            if (article == null)
            {
                throw new NullableArticleException();
            }
            await _articleRepository.DeleteAsync(article);
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _articleRepository.GetAllAsync();
        }

        public async Task<Article> GetArticleByIdAsync(Guid articleId)
        {
            var article = await _articleRepository.GetByIdAsync(articleId);
            if (article == null)
            {
                throw new NullableArticleException();
            }
            return article;
        }

        public async Task<IEnumerable<Article>> GetArticlesByBlogNameAsync(string blogName)
        {
            var articles = await _articleRepository.GetAllAsync();
            articles = articles.Where(a => a.BlogTitle == blogName);
            return articles;
        }

        public async Task<Article> GetArticleByTagAsync(Tag tag)
        {
            var articles = await _articleRepository.GetAllAsync();
            var article = articles.FirstOrDefault(a => a.Tags == tag);
            return article;
        }

        public async Task<Article> GetArticleByNameAsync(string articleName)
        {
            var articles = await _articleRepository.GetAllAsync();
            var article = articles.FirstOrDefault(a => a.Title == articleName);
            return article;
        }

        public async Task<Article> UpdateArticleAsync(Article articleToUpdate)
        {
            var article = await GetArticleByIdAsync(articleToUpdate.Id);
            if (article == null)
            {
                throw new NullableArticleException();
            }
            article.Text = articleToUpdate.Text;
            article.Title = articleToUpdate.Title;
            await _articleRepository.UpdateAsync(article);
            return article;
        }

    }
}
