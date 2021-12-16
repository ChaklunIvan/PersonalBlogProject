using PersonalBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task CreateTokenAsync(RefreshToken token);
        Task<RefreshToken> GetByTokenAsync(string token);
    }
}
