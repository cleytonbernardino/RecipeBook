using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.API.Filter
{
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        public AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserReadOnlyRepository userReadOnlyRepository)
        {
            _accessTokenValidator = accessTokenValidator;
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                string token = TokenOnRequest(context);

                Guid userIndentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

                bool exist = await _userReadOnlyRepository.ExistActiveUserWithIndentifier(userIndentifier);

                if (!exist)
                    throw new RecipeBookException(ResourceMessagesException.USER_DOES_NOT_HAVE_PERMISSION);
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new UnauthorizedObjectResult(
                    new ResponseErrorJson(ResourceMessagesException.EXPIRED_TOKEN)
                    {
                        TokenIsExpired = true
                    }
                );
            }
            catch (RecipeBookException e)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(e.Message));
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_DOES_NOT_HAVE_PERMISSION));
            }
        }

        private static string TokenOnRequest(AuthorizationFilterContext context)
        {
            string? authorization = context.HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authorization))
                throw new RecipeBookException(ResourceMessagesException.NO_TOKEN);
            return authorization["Bearer ".Length..].Trim();
        }
    }
}
