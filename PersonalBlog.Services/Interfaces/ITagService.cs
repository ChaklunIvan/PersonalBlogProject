using PersonalBlog.Data.Models;

namespace PersonalBlog.Services.Interfaces
{
    public interface ITagService
    {
        Task<Tag> CreateTagAsync(Tag tagToCreate);
        Task<Tag> UpdateTagAsync(Tag tagToUpdate);
        Task DeleteTagAsync(Guid tagId);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
    }
}
