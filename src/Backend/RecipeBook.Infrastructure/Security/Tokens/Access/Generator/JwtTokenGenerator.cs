using Microsoft.IdentityModel.Tokens;
using RecipeBook.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RecipeBook.Infrastructure.Security.Tokens.Access.Generator
{
    public class JwtTokenGenerator : JwtTokenHandle, IAccessTokenGenerator
    {
        private readonly uint _expirationTimeMinutes;
        private readonly string _signingKey;

        public JwtTokenGenerator(uint expirationTimeMinutes, string signingKey)
        {
            _expirationTimeMinutes = expirationTimeMinutes;
            _signingKey = signingKey;
        }

        public string Generate(Guid userIdentifier, bool expired = false)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, userIdentifier.ToString())
            };

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            if (expired)
            {
                tokenDescriptor.NotBefore = DateTime.UtcNow.AddMinutes(-10);
                tokenDescriptor.Expires = DateTime.UtcNow.AddMinutes(-5);
            }

            JwtSecurityTokenHandler tokenHandle = new();
            SecurityToken securityToken = tokenHandle.CreateToken(tokenDescriptor);

            return tokenHandle.WriteToken(securityToken);
        }
    }
}
