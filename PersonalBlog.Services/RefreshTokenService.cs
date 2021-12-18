using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Data.Models;
using PersonalBlog.Services.Interfaces;
using System.Linq;
using PersonalBlog.Data.Exceptions;

namespace PersonalBlog.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IGenericRepository<RefreshToken> _tokenRepository;

        public RefreshTokenService(IGenericRepository<RefreshToken> tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task CreateTokenAsync(RefreshToken token)
        {
            await _tokenRepository.CreateAsync(token);
        }

        public async Task DeleteAllTokens(Guid userId)
        {
            var tokens = await _tokenRepository.GetAllAsync();
            tokens = tokens.Where(t => t.UserId == userId).ToList();
            if (tokens == null)
            {
                throw new NullableTokenException();
            }
            await _tokenRepository.DeleteAllAsync(tokens);
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var tokens = await _tokenRepository.GetAllAsync();
            var resultToken = tokens.FirstOrDefault(t => t.Token == token);
            if(resultToken == null)
            {
                throw new NullableTokenException();
            }
            return resultToken;
        }
    }
}
