using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Microsoft.AspNetCore.Http;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update
{
    public class UpdateUserInvalidTokenTest : RecipeBookClassFixture
    {
        private const string METHOD = "user";

        private readonly Guid _userIndentifier;

        public UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Invalid_Token(string culture)
        {
            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            HttpResponseMessage response = await DoPut(METHOD, request, "TokenInvalid", culture);
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);

            JsonElement jsonElement = await GetJsonElementAsync(response);

            var erros = jsonElement.GetProperty("errors").EnumerateArray();
            Assert.Single(erros);

            string expected_message = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PERMISSION", new CultureInfo(culture))!;
            Assert.Equal(expected_message, erros.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Without_Token(string culture)
        {
            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            HttpResponseMessage response = await DoPut(METHOD, request, "", culture);
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

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            HttpResponseMessage response = await DoPut(METHOD, request, token, culture);

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

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            HttpResponseMessage response = await DoPut(METHOD, request, token, culture);

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
