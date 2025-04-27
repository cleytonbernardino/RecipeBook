using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
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
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPut(METHOD, request, token);
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            // Trying to log in to verify that the password was actually changed
            RequestLoginJson loginRequest = new()
            {
                Email = _email,
                Password = _password
            };
            var loginResponse = await DoPost("login", loginRequest);
            loginResponse.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            loginRequest.Password = request.NewPassword;
            loginResponse = await DoPost("login", loginRequest);
            loginResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
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

            var response = await DoPut(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_CurrentPassword_Different(string culture)
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPut(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_DIFFERENT_FROM_CURRENT_PASSWORD", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }
    }
}
