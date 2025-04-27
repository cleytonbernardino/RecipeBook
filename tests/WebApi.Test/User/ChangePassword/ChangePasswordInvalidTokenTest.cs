using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.ChangePassword
{
    public class ChangePasswordInvalidTokenTest : RecipeBookClassFixture
    {
        private const string METHOD = "user/change-password";

        private readonly Guid _userIndentifier;

        public ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Invalid_Token(string culture)
        {
            RequestChangePasswordJson request = new();
            var response = await DoPut(METHOD, request, token: "TokenInvalid", culture);
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
            RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();
            
            var response = await DoPut(METHOD, request, "", culture);
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

            var request = RequestChangePasswordJsonBuilder.Build();
            
            var response = await DoPut(METHOD, request, token, culture);
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

            var request = RequestChangePasswordJsonBuilder.Build();

            var response = await DoPut(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            JsonElement jsonElement = await GetJsonElementAsync(response);

            // MsgError
            var errors = jsonElement.GetProperty("errors").EnumerateArray();
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EXPIRED_TOKEN", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);

            // Status Expired
            bool tokenIsExpired = jsonElement.GetProperty("tokenIsExpired").GetBoolean();
            tokenIsExpired.ShouldBeTrue();
        }
    }
}
