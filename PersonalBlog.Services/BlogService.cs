using PersonalBlog.Data.Exceptions;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Services
{
    public class BlogService : IBlogService
    {
        private readonly IGenericRepository<Blog> _blogRepository;

        public BlogService(IGenericRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<Blog> CreateBlogAsync(Blog blogToCreate)
        {
            var blog = new Blog()
            {
                Title = blogToCreate.Title,
                Articles = blogToCreate.Articles,
                User = blogToCreate.User
            };
            await _blogRepository.CreateAsync(blog);
            return blog;
        }

        public async Task DeleteBlogAsync(Guid blogId)
        {
            var blog = await _blogRepository.GetByIdAsync(blogId);
            if(blog == null)
            {
                throw new NullableBlogException();
            }
            await _blogRepository.DeleteAsync(blog);
        }

        public async Task<IEnumerable<Blog>> GetAllBlogAsync()
        {
            return await _blogRepository.GetAllAsync();
        }

        public async Task<Blog> GetBlogByNameAsync(string blogTitle)
        {
            var blogs = await _blogRepository.GetAllAsync();
            var blog = blogs.FirstOrDefault(b => b.Title == blogTitle);
            return blog;
        }

        public async Task<IEnumerable<Blog>> GetBlogsByUser(User user)
        {
            var blogs = await _blogRepository.GetAllAsync();
            var blog = blogs.Where(b => b.User == user);
            return blog.ToList();
        }

        public async Task<Blog> UpdateBlogAsync(Blog blogToUpdate)
        {
            var blog = await _blogRepository.GetByIdAsync(blogToUpdate.Id);
            if (blog == null)
            {
                throw new NullableBlogException();
            }
            blog.Title = blogToUpdate.Title;
            await _blogRepository.UpdateAsync(blog);
            return blog;
        }
    }
}
