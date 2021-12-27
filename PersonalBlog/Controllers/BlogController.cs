using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Models.Requests;
using PersonalBlog.Data.Responses;
using PersonalBlog.Services.Interfaces;
using System.Security.Claims;

namespace PersonalBlog.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;
        public BlogController(IBlogService blogService, IUserService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }

        [HttpPost("createblog")]
        [Authorize]
        public async Task<IActionResult> CreateBlog([FromBody] Blog blogToCreate)
        {
            if (!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };
            var user = await _userService.GetUserByIdAsync(userId);
            blogToCreate.User = user;

            await _blogService.CreateBlogAsync(blogToCreate);
            return Ok();
        }

        [HttpPost("updateblog")]
        [Authorize]
        public async Task<IActionResult> UpdateBlog([FromBody] Blog blogToUpdate)
        {
            if (!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };
            var user = await _userService.GetUserByIdAsync(userId);
            var blog = await _blogService.GetBlogByNameAsync(blogToUpdate.Title);

            if (blog == null)
            {
                return BadRequest(new ErrorResponse("Blog doesn't exist"));
            }
            if (user != blog.User)
            {
                return BadRequest(new ErrorResponse("Only the blog owner can edit a blog"));
            }

            await _blogService.UpdateBlogAsync(blogToUpdate);
            return Ok();
        }

        [HttpGet("getallblogs")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllBlogAsync();
            return Ok(blogs);
        }

        [HttpGet("getblog")]
        public async Task<IActionResult> GetBlogByName([FromBody]string blogName)
        {
            var blog = await _blogService.GetBlogByNameAsync(blogName);
            return Ok(blog);
        }

        [HttpDelete("deleteblog")]
        [Authorize]
        public async Task<IActionResult> DeleteBlog([FromBody] Guid blogId)
        {
            if (!Guid.TryParse(HttpContext.User.FindFirstValue("id"), out Guid userId))
            {
                return Unauthorized();
            };
            var user = await _userService.GetUserByIdAsync(userId);
            var blogs = await _blogService.GetBlogsByUser(user);
            var blog = blogs.FirstOrDefault(b => b.Id == blogId);
            if (blog == null)
            {
                return BadRequest(new ErrorResponse("Blog doesn't exist"));
            }
            if (user != blog.User)
            {
                return BadRequest(new ErrorResponse("Only the blog owner can edit a blog"));
            }
            await _blogService.DeleteBlogAsync(blogId);
            return Ok();
        }
        
        [HttpDelete("deleteblogbyadmin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBlogByAdmin([FromBody] Guid blogId)
        {
            await _blogService.DeleteBlogAsync(blogId);
            return Ok();
        }
    }
}
