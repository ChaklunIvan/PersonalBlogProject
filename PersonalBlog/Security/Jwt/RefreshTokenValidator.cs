using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PersonalBlog.Security.Jwt
{
    public class RefreshTokenValidator
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;

        public RefreshTokenValidator(AuthenticationConfiguration authenticationConfiguration)
        {
            _authenticationConfiguration = authenticationConfiguration;
        }

        public bool Validate(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationConfiguration.RefreshTokenSecret)),
                ValidIssuer = _authenticationConfiguration.Issuer,
                ValidAudience = _authenticationConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validateToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
       
}
