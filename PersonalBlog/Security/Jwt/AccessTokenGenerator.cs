﻿using Microsoft.IdentityModel.Tokens;
using PersonalBlog.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalBlog.Security.Jwt
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public AccessTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim("id",user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
            };
            return _tokenGenerator.GenerateToken(
                _configuration.AccessTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                _configuration.AccessTokenExpirationMinutes,
                claims);
        }
    }
}
