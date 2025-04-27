using CommonTestUtilities.Tokens;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
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
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var erros = await GetErrorList(response);
            string expected_message = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PERMISSION", new CultureInfo(culture))!;

            erros.ShouldHaveSingleItem();
            erros.First().ToString().ShouldBe(expected_message);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Without_Token(string culture)
        {
            var response = await DoGet(METHOD, token: "", culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var erros = await GetErrorList(response);
            string expected_message = ResourceMessagesException.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture))!;

            erros.ShouldHaveSingleItem();
            erros.First().ToString().ShouldBe(expected_message);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Without_User(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(METHOD, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var erros = await GetErrorList(response);
            string expected_message = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PERMISSION", new CultureInfo(culture))!;

            erros.ShouldHaveSingleItem();
            erros.First().ToString().ShouldBe(expected_message);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Expired(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier, true);

            var response = await DoGet(METHOD, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            JsonElement jsonElement = await GetJsonElementAsync(response);

            // MsgError
            var errors = jsonElement.GetProperty("errors").EnumerateArray();
            string expected_message = ResourceMessagesException.ResourceManager.GetString("EXPIRED_TOKEN", new CultureInfo(culture))!;
            
            errors.ShouldHaveSingleItem();
            errors.First().ToString().ShouldBe(expected_message);

            // Status Expired
            bool tokenIsExpiredors = jsonElement.GetProperty("tokenIsExpired").GetBoolean();
            Assert.True(tokenIsExpiredors);
        }
    }
}
