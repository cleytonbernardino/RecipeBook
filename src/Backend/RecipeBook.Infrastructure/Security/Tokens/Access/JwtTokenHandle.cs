using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RecipeBook.Infrastructure.Security.Tokens.Access
{
    public abstract class JwtTokenHandle
    {
        protected SymmetricSecurityKey SecurityKey(string signingKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(signingKey);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
