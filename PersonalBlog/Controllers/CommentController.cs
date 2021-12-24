using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Controllers
{
    [Route("api/blog/article/comment")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("createcomment")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] Comment commentToCreate)
        {
            var comment = await _commentService.CreateCommentAsync(commentToCreate);
            return Ok(comment);
        }

        [HttpGet("getallcomments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        [HttpPost("updatecomment")]
        [Authorize]
        public async Task<IActionResult> UpdateComment([FromBody] Comment commentToUpdate)
        {
            var comment = await _commentService.UpdateCommentAsync(commentToUpdate);
            return Ok(comment);
        }

        [HttpDelete("deletecomment")]
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromBody] Guid commentId)
        {
            await _commentService.DeleteCommentAsync(commentId);
            return Ok();
        }
    }
}
