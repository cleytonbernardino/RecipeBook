using CommonTestUtilities.Tokens;
using Microsoft.AspNetCore.Http;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Dashboard
{
    public class DashboardInvalidTokenTest : RecipeBookClassFixture
    {
        private const string METHOD = "dashboard";

        private readonly Guid _userIndentifier;

        public DashboardInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Invalid_Token(string culture)
        {
            var response = await DoGet(METHOD, token: "TokenInvalid", culture);
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);

            var erros = await GetErrorList(response);
            Assert.Single(erros);

            string expected_message = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PERMISSION", new CultureInfo(culture))!;
            Assert.Equal(expected_message, erros.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Without_Token(string culture)
        {
            var response = await DoGet(METHOD, token: "", culture);
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);

            JsonElement jsonElement = await GetJsonElementAsync(response);

            var erros = jsonElement.GetProperty("errors").EnumerateArray();
            Assert.Single(erros);

            string expected_message = ResourceMessagesException.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture))!;
            Assert.Equal(expected_message, erros.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Without_User(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(METHOD, token, culture);
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);

            JsonElement jsonElement = await GetJsonElementAsync(response);

            var erros = jsonElement.GetProperty("errors").EnumerateArray();
            Assert.Single(erros);

            string expected_message = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PERMISSION", new CultureInfo(culture))!;
            Assert.Equal(expected_message, erros.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Expired(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier, true);

            var response = await DoGet(METHOD, token, culture);
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);

            JsonElement jsonElement = await GetJsonElementAsync(response);

            // MsgError
            var errors = jsonElement.GetProperty("errors").EnumerateArray();
            Assert.Single(errors);

            string expected_message = ResourceMessagesException.ResourceManager.GetString("EXPIRED_TOKEN", new CultureInfo(culture))!;
            Assert.Equal(expected_message, errors.First().ToString());

            // Status Expired
            bool tokenIsExpiredors = jsonElement.GetProperty("tokenIsExpired").GetBoolean();
            Assert.True(tokenIsExpiredors);
        }
    }
}
