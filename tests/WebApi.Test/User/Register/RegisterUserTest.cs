using CommonTestUtilities.Requests;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest : RecipeBookClassFixture
    {
        private const string METHOD = "user";

        public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Success()
        {
            var request = RequestUserJsonBuilder.Build();

            var response = await DoPost(METHOD, request);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var jsonElement = await GetJsonElementAsync(response);

            string? name = jsonElement.GetProperty("name").GetString();
            request.Name.ShouldBe(name);
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Name_Empty_Is_Returning_Correct_Error(string culture)
        {
            var request = RequestUserJsonBuilder.Build();
            request.Name = "";

            var response = await DoPost(method: METHOD, request: request, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Empty_Is_Returning_Correct_Error(string culture)
        {
            var request = RequestUserJsonBuilder.Build();
            request.Email = string.Empty;

            var response = await DoPost(method: METHOD, request: request, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Invalid_Is_Returning_Correct_Error(string culture)
        {
            var request = RequestUserJsonBuilder.Build();
            request.Email = "email.com";

            var response = await DoPost(method: METHOD, request: request, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_INVALID", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Already_Exists_Is_Returning_Correct_Error(string culture)
        {
            var request = RequestUserJsonBuilder.Build();

            await DoPost(method: METHOD, request: request);
            var response = await DoPost(method: METHOD, request: request, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_IN_USE", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Password_Lenght_Is_Returning_Correct_Error(string culture)
        {
            var request = RequestUserJsonBuilder.Build();
            request.Password = "123";

            var response = await DoPost(method: METHOD, request: request, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_LENGTH_INVALID", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }
    }
}
