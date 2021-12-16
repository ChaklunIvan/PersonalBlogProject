using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Domain.Models;
using PersonalBlog.Services.Interfaces;
using System.Linq;

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

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var tokens = await _tokenRepository.GetAllAsync();
            var resultToken = tokens.FirstOrDefault(t => t.Token == token);
            if(resultToken == null)
            {
                throw new ArgumentException("Invalid token");
            }
            return resultToken;
        }
    }
}
