using PersonalBlog.Data.Models;


namespace PersonalBlog.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateTokenAsync(RefreshToken token);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task DeleteAllTokens(Guid userId);
    }
}
