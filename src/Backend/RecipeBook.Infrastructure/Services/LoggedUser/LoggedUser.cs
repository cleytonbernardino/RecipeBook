using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RecipeBook.Infrastructure.Services.LoggedUser
{
    public class LoggedUser : ILoggedUser
    {
        private readonly RecipeBookDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(RecipeBookDbContext dbContext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
        }

        public async Task<User> User()
        {
            string token = _tokenProvider.Value();

            JwtSecurityTokenHandler tokenHandle = new();

            JwtSecurityToken jwtSecurityToken = tokenHandle.ReadJwtToken(token);

            string indentifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            Guid userIndentifier = Guid.Parse(indentifier);

            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.UserIdentifier.Equals(userIndentifier) && user.Active);
        }
    }
}
