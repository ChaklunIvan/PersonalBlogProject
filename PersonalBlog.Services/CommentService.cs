using PersonalBlog.Data.Exceptions;
using PersonalBlog.Data.Models;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Services.Interfaces;

namespace PersonalBlog.Services
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepository<Comment> _commentRepository;

        public CommentService(IGenericRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Comment> CreateCommentAsync(Comment commentToCreate)
        {
            var comment = new Comment() 
            { 
                Value = commentToCreate.Value,
                Article = commentToCreate.Article,
                User = commentToCreate.User
            };
            await _commentRepository.CreateAsync(comment);
            return comment;
        }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if(comment == null)
            {
                throw new NullableCommentException();
            }
            await _commentRepository.DeleteAsync(comment);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByArticleAsync(Guid articleId)
        {
            var comments = await _commentRepository.GetAllAsync();
            return comments.Where(c => c.Article.Id == articleId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(Guid userId)
        {
            var comments = await _commentRepository.GetAllAsync();
            return comments.Where(c => c.User.Id == userId);
        }

        public async Task<Comment> UpdateCommentAsync(Comment commentToUpdate)
        {
            var comment = await _commentRepository.GetByIdAsync(commentToUpdate.Id);
            if (comment == null)
            {
                throw new NullableCommentException();
            };
            comment.Value = commentToUpdate.Value;
            await _commentRepository.UpdateAsync(comment);
            return comment;
        }
    }
}
