using Microsoft.IdentityModel.Tokens;
using RecipeBook.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RecipeBook.Infrastructure.Security.Tokens.Access.Validator
{
    public class JwtTokenValidator : JwtTokenHandle, IAccessTokenValidator
    {
        private readonly string _signingKey;

        public JwtTokenValidator(uint expirationTimeMinutes, string signingKey)
        {
            _signingKey = signingKey;
        }

        public Guid ValidateAndGetUserIdentifier(string token)
        {
            TokenValidationParameters validationParameter = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = SecurityKey(_signingKey),
                ClockSkew = new TimeSpan(0),
            };

            JwtSecurityTokenHandler tokenHandle = new();

            ClaimsPrincipal principal = tokenHandle.ValidateToken(token, validationParameter, out _);

            string userIndentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            return Guid.Parse(userIndentifier);
        }
    }
}
