using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Responses;
using PersonalBlog.Services.Interfaces;
using System.Security.Claims;

namespace PersonalBlog.Controllers
{
    [Route("api/blog/article")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        public ArticleController(IArticleService articleService, IBlogService blogService, IUserService userService)
        {
            _articleService = articleService;
            _blogService = blogService;
            _userService = userService;
        }

        [HttpPost("createarticle")]
        [Authorize]
        public async Task<IActionResult> CreateArticle([FromBody] Article articleToCreate)
        {
            if(!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };

            var user = await _userService.GetUserByIdAsync(userId);
            var blogs = await _blogService.GetBlogsByUser(user);
            if (blogs == null)
            {
                return BadRequest(new ErrorResponse("There is no blogs for this user"));
            }

            var blog = blogs.FirstOrDefault(b => b.Title == articleToCreate.BlogTitle);
            if (blog == null)
            {
                return BadRequest(new ErrorResponse("Blog does not exist"));
            }

            articleToCreate.Blog = blog;
            await _articleService.CreateArticleAsync(articleToCreate);
            return Ok();
        }

        [HttpPost("updatearticle")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle([FromBody] Article articleToUpdate)
        {
            if (!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };

            var user = await _userService.GetUserByIdAsync(userId);
            var blog = await _blogService.GetBlogByNameAsync(articleToUpdate.BlogTitle);
            if (blog == null)
            {
                return BadRequest(new ErrorResponse("Blog does not exist"));
            }
            if (user != blog.User)
            {
                return BadRequest(new ErrorResponse("Only the blog owner can edit a blog"));
            }

            await _articleService.UpdateArticleAsync(articleToUpdate);
            return Ok();
        }

        [HttpGet("getallarticles")]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<IEnumerable<Article>>> GetAllArticles()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return Ok(articles);
        }

        [HttpGet("getalltags")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Article>>> GetAllTags()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            var tags = articles.SelectMany(x => x.Tags);
            return Ok(tags);
        }

        [HttpGet("getarticlesbyblog")]
        public async Task<ActionResult<IEnumerable<Article>>> GetAllArticlesByBlog(string blogName)
        {
            var articles = await _articleService.GetArticlesByBlogNameAsync(blogName);
            return Ok(articles);
        }

        [HttpGet("getarticle")]
        public async Task<IActionResult> GetArticle([FromBody]Guid articleId)
        {
            var blog = await _articleService.GetArticleByIdAsync(articleId);
            return Ok(blog);
        }

        [HttpGet("getarticlebytag")]
        public async Task<IActionResult> GetArticleByTag([FromBody] string tag)
        {
            var article = await _articleService.GetArticleByTagAsync(tag);
            if(article == null)
            {
                return BadRequest(new ErrorResponse("There is no article for this tag"));
            }
            return Ok(article);
        }

        [HttpDelete("deletearticle")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle([FromBody] Guid articleId)
        {
            if (!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };
            var article = await _articleService.GetArticleByIdAsync(articleId);
            var user = await _userService.GetUserByIdAsync(userId);
            var blogs = await _blogService.GetBlogsByUser(user);
            if (blogs == null)
            {
                return BadRequest(new ErrorResponse("There is no blogs for this user"));
            }
            var blog = blogs.FirstOrDefault(b => b.Id == article.Blog.Id);
            if (user != blog.User)
            {
                return BadRequest(new ErrorResponse("Only the blog owner can edit a blog"));
            }

            await _articleService.DeleteArticleAsync(article.Id);
            return Ok();
        }

        [HttpDelete("deletearticlebyadmin")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> DeleteArticleByAdmin([FromBody] Guid articleId)
        {
            await _articleService.DeleteArticleAsync(articleId);
            return Ok();
        }

    }
}
