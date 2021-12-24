using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost("createblog")]
        [Authorize]
        public async Task<IActionResult> CreateBlog([FromBody] Blog blogToCreate)
        {
            var blog = await _blogService.CreateBlogAsync(blogToCreate);
            return Ok(blog);
        }

        [HttpPost("updateblog")]
        [Authorize]
        public async Task<IActionResult> UpdateBlog([FromBody] Blog blogToUpdate)
        {
            var blog = await _blogService.UpdateBlogAsync(blogToUpdate);
            return Ok(blog);
        }

        [HttpGet("getallblogs")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllBlogAsync();
            return Ok(blogs);
        }

        [HttpGet("getblog")]
        public async Task<IActionResult> GetBlogByName(string blogName)
        {
            var blog = await _blogService.GetBlogByNameAsync(blogName);
            return Ok(blog);
        }
        
        [HttpDelete("deleteblog")]
        [Authorize]
        public async Task<IActionResult> DeleteBlog([FromBody] Guid blogId)
        {
            await _blogService.DeleteBlogAsync(blogId);
            return Ok();
        }
    }
}
