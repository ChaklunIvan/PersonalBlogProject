using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Controllers
{
    [Route("api/blog/article")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpPost("createarticle")]
        [Authorize]
        public async Task<IActionResult> CreateArticle([FromBody] Article articleToCreate)
        {
            var article =  await _articleService.CreateArticleAsync(articleToCreate);
            return Ok(article);
        }

        [HttpPost("updatearticle")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle([FromBody] Article articleToUpdate)
        {
            var article = await _articleService.UpdateArticleAsync(articleToUpdate);
            return Ok(article);
        }

        [HttpGet("getallarticles")]
        public async Task<ActionResult<IEnumerable<Article>>> GetAllArticles()
        {
            var blogs = await _articleService.GetAllArticlesAsync();
            return Ok(blogs);
        }

        [HttpGet("getarticle")]
        public async Task<IActionResult> GetArticle([FromBody]Guid blogId)
        {
            var blog = await _articleService.GetArticleByIdAsync(blogId);
            return Ok(blog);
        }

        [HttpGet("getarticlebytag")]
        public async Task<IActionResult> GetArticleByTag([FromBody] Tag tag)
        {
            var article = await _articleService.GetArticleByTag(tag);
            return Ok(article);
        }

        [HttpGet("getarticlebytext")]
        public async Task<IActionResult> GetArticleByText([FromBody]string text)
        {
            var article = await _articleService.GetArticleByTextAsync(text);
            return Ok(article);
        }

        [HttpDelete("deletearticle")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle([FromBody] Guid articleId)
        {
            await _articleService.DeleteArticleAsync(articleId);
            return Ok();
        }
    }
}
