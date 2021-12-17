﻿using PersonalBlog.Data.Models;


namespace PersonalBlog.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task CreateTokenAsync(RefreshToken token);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task DeleteAllTokens(string userId);
    }
}
