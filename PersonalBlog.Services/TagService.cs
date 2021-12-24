using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Services
{
    public class TagService : ITagService
    {
        private readonly IGenericRepository<Tag> _tagRepository;

        public TagService(IGenericRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Tag> CreateTagAsync(Tag tagToCreate)
        {
            await _tagRepository.CreateAsync(tagToCreate);
            return tagToCreate;
        }

        public async Task DeleteTagAsync(Guid tagId)
        {
            var tag = await _tagRepository.GetByIdAsync(tagId);
            if(tag == null)
            {
                throw new Exception();
            }
            await _tagRepository.DeleteAsync(tag);
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.GetAllAsync();
        }

        public async Task<Tag> UpdateTagAsync(Tag tagToUpdate)
        {
            var tag = await _tagRepository.GetByIdAsync(tagToUpdate.Id);
            if (tag == null)
            {
                throw new Exception();
            }
            tag.Value = tagToUpdate.Value;
            await _tagRepository.UpdateAsync(tag);
            return tag;
        }
    }
}
