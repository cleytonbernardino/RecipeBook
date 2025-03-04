using RecipeBook.Domain.Security.Tokens;

namespace RecipeBook.API.Token
{
    public class HttpContextTokenValue : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Value()
        {
            string authorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
            return authorization["Bearer".Length..];
        }
    }
}
