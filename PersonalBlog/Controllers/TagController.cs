using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Responses;
using PersonalBlog.Services.Interfaces;
using System.Security.Claims;

namespace PersonalBlog.Controllers
{
    [Route("api/blog/article/tag")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IUserService _userService;
        private readonly IBlogService _blogService;
        private readonly IArticleService _articleService;

        public TagController(ITagService tagService, IUserService userService, IBlogService blogService, IArticleService articleService)
        {
            _tagService = tagService;
            _userService = userService;
            _blogService = blogService;
            _articleService = articleService;
        }

        [HttpPost("createtag")]
        [Authorize]
        public async Task<IActionResult> CreateTag([FromBody] Tag tagToCreate)
        {
            var tag = await _tagService.CreateTagAsync(tagToCreate);
            return Ok(tag);
        }

        [HttpPost("updatetag")]
        [Authorize]
        public async Task<IActionResult> UpdateTag([FromBody] Tag tagToUpdate)
        {
            var tag = await _tagService.UpdateTagAsync(tagToUpdate);
            return Ok(tag);
        }

        [HttpGet("getalltags")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetAllTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteTag([FromBody] Guid tagId)
        {
            await _tagService.DeleteTagAsync(tagId);
            return Ok();
        }
    }
}
