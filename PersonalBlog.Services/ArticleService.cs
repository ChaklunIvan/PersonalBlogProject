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
            await _articleRepository.CreateAsync(articleToCreate);
            return articleToCreate;
        }

        public async Task DeleteArticleAsync(Guid articleId)
        {
            var article =  await _articleRepository.GetByIdAsync(articleId);
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

        public Task<Article> GetArticleByTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public Task<Article> GetArticleByTextAsync(string text)
        {
            throw new NotImplementedException();
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
