using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Models.Requests;
using PersonalBlog.Data.Responses;
using PersonalBlog.Services.Interfaces;
using System.Security.Claims;

namespace PersonalBlog.Controllers
{
    [Route("api/blog/article/comment")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IArticleService _articleService;

        public CommentController(ICommentService commentService, IUserService userService, IArticleService articleService)
        {
            _commentService = commentService;
            _userService = userService;
            _articleService = articleService;
        }

        [HttpPost("createcomment")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CommentRequest commentRequest)
        {
            if (!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };
            var user = await _userService.GetUserByIdAsync(userId);
            var article = await _articleService.GetArticleByNameAsync(commentRequest.Article);
            if (article == null)
            {
                return BadRequest(new ErrorResponse("Article not found"));
            }

            var comment = new Comment()
            {
                Value = commentRequest.Value,
                User = user,
                Article = article,
            };

            return Ok(comment);
        }

        [HttpGet("getallcommentsbyarticle")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllCommentsByArticle(Guid articleId)
        {
            var comments = await _commentService.GetCommentsByArticleAsync(articleId);
            return Ok(comments);
        }

        [HttpGet("getallcommentsbyme")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllCommentsByMe()
        {
            if (!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };
            var comments = await _commentService.GetCommentsByUserAsync(userId);
            return Ok(comments);
        }

        [HttpGet("getallcommentsbyuser")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllCommentsByUser(Guid userId)
        {
            var comments = await _commentService.GetCommentsByUserAsync(userId);
            return Ok(comments);
        }

        [HttpPost("updatecomment")]
        [Authorize]
        public async Task<IActionResult> UpdateComment([FromBody] Comment commentToUpdate)
        {
            var article = commentToUpdate.Article;
            if (!article.Comments.Contains(commentToUpdate))
            {
                return BadRequest(new ErrorResponse("No comments for this article"));
            }
            var comment = await _commentService.UpdateCommentAsync(commentToUpdate);
            return Ok(comment);
        }

        [HttpDelete("deletecomment")]
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromBody] Guid commentId)
        {
            if (!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };
            if(_commentService.GetCommentsByUserAsync(userId) == null)
            {
                return BadRequest(new ErrorResponse("No comments for this user"));
            }
            await _commentService.DeleteCommentAsync(commentId);
            return Ok();
        }

        [HttpDelete("deletecommentbyadmin")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> DeleteCommentByAdmin([FromBody] Guid commentId)
        {
            await _commentService.DeleteCommentAsync(commentId);
            return Ok();
        }
    }
}
