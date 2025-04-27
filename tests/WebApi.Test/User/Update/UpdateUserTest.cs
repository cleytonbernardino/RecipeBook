using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update
{
    public class UpdateUserTest : RecipeBookClassFixture
    {
        private const string METHOD = "user";

        private readonly Guid _userIndentifier;

        public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Fact]
        public async Task Success()
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var request = RequestUpdateUserJsonBuilder.Build();

            var response = await DoPut(METHOD, request, token);
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = "";

            var response = await DoPut(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Email_Empty(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "";

            var response = await DoPut(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Email_Invalid(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "email.com";

            var response = await DoPut(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_INVALID", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Email_Already_In_Use(string culture)
        {
            // Creating another user to use the same email
            var userRequest = RequestUserJsonBuilder.Build();
            await DoPost(METHOD, userRequest);

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = userRequest.Email;

            var response = await DoPut(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_IN_USE", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }
    }
}
