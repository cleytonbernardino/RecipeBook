using System.Globalization;

namespace RecipeBook.API.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            CultureInfo[] supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures);
            CultureInfo cultureInfo = new("en");

            string? requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(requestCulture) == false && supportedLanguages.Any(c => c.Name == requestCulture))
            {
                cultureInfo = new CultureInfo(requestCulture);
            }
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
}
