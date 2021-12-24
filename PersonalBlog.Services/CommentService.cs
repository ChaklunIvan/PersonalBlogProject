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
            await _commentRepository.CreateAsync(commentToCreate);
            return commentToCreate;
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

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetAllAsync();
            return comments;
        }

        public async Task<Comment> UpdateCommentAsync(Comment commentToUpdate)
        {
            var comment = await _commentRepository.GetByIdAsync(commentToUpdate.Id);
            if(comment == null)
            {
                throw new NullableCommentException();
            }
            await _commentRepository.UpdateAsync(comment);
            return comment;
        }
    }
}
