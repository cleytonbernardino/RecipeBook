using CommonTestUtilities.Tokens;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Profile
{
    public class RegisterRecipeInvalidTokenTest : RecipeBookClassFixture
    {
        private const string METHOD = "user";

        private readonly Guid _userIndentifier;

        public RegisterRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Invalid_Token(string culture)
        {
            var response = await DoGet(METHOD, "TokenInvalid", culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PERMISSION", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Without_Token(string culture)
        {
            var response = await DoGet(METHOD, "", culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Without_User(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
            var response = await DoGet(METHOD, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var errors = await GetErrorList(response);

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PERMISSION", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
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
            errors.ShouldHaveSingleItem();

            string expected_message = ResourceMessagesException.ResourceManager.GetString("EXPIRED_TOKEN", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expected_message);

            // Status Expired
            bool tokenIsExpired = jsonElement.GetProperty("tokenIsExpired").GetBoolean();
            tokenIsExpired.ShouldBeTrue();
        }
    }
}
