using PersonalBlog.Data.Models;

namespace PersonalBlog.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> CreateCommentAsync(Comment commentToCreate);
        Task<Comment> UpdateCommentAsync(Comment commentToUpdate);
        Task DeleteCommentAsync(Guid commentId);
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
    }
}
