using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Microsoft.AspNetCore.Http;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.ChangePassword
{
    public class ChangePasswordTest : RecipeBookClassFixture
    {
        private const string METHOD = "user/change-password";

        public readonly string _email;
        public readonly string _password;
        public readonly Guid _userIndentifier;

        public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _email = factory.GetEmail();
            _password = factory.GetPassword();
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Fact]
        public async Task Success()
        {
            RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            HttpResponseMessage response = await DoPut(METHOD, request, token);

            Assert.Equal(StatusCodes.Status204NoContent, ((int)response.StatusCode));

            // Trying to log in to verify that the password was actually changed
            RequestLoginJson loginRequest = new()
            {
                Email = _email,
                Password = _password
            };
            HttpResponseMessage loginResponse = await DoPost("login", loginRequest);
            Assert.Equal(StatusCodes.Status401Unauthorized, ((int)loginResponse.StatusCode));

            loginRequest.Password = request.NewPassword;
            loginResponse = await DoPost("login", loginRequest);
            Assert.Equal(StatusCodes.Status200OK, ((int)loginResponse.StatusCode));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_NewPassword_Empyt(string culture)
        {
            RequestChangePasswordJson request = new()
            {
                Password = _password,
                NewPassword = ""
            };

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            HttpResponseMessage response = await DoPut(METHOD, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, ((int)response.StatusCode));

            JsonElement jsonElement = await GetJsonElementAsync(response);
            var errors = jsonElement.GetProperty("errors").EnumerateArray();

            Assert.Single(errors);

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture))!;
            Assert.Equal(expectedMessage, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_CurrentPassword_Different(string culture)
        {
            RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            HttpResponseMessage response = await DoPut(METHOD, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, ((int)response.StatusCode));

            JsonElement jsonElement = await GetJsonElementAsync(response);
            var errors = jsonElement.GetProperty("errors").EnumerateArray();

            Assert.Single(errors);

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_DIFFERENT_FROM_CURRENT_PASSWORD", new CultureInfo(culture))!;
            Assert.Equal(expectedMessage, errors.First().ToString());
        }
    }
}
