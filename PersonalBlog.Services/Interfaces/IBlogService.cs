using PersonalBlog.Data.Models;

namespace PersonalBlog.Services.Interfaces
{
    public interface IBlogService
    {
        Task<Blog> CreateBlogAsync(Blog blogToCreate);
        Task<Blog> UpdateBlogAsync(Blog blogToUpdate);
        Task DeleteBlogAsync(Guid blogId);
        Task<Blog> GetBlogByNameAsync(string blogTitle);
        Task<IEnumerable<Blog>> GetAllBlogAsync();
    }
}
