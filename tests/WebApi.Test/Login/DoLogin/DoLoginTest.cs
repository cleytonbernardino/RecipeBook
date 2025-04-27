using CommonTestUtilities.Requests;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : RecipeBookClassFixture
    {
        private const string METHOD = "login";

        private readonly string _email;
        private readonly string _password;
        private readonly string _name;

        public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _email = factory.GetEmail();
            _password = factory.GetPassword();
            _name = factory.GetName();
        }

        [Fact]
        public async Task Success()
        {
            RequestLoginJson request = new()
            {
                Email = _email,
                Password = _password
            };

            var response = await DoPost(method: METHOD, request: request);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var jsonELement = await GetJsonElementAsync(response);

            string? name = jsonELement.GetProperty("name").GetString();

            string? accessToken = jsonELement.GetProperty("tokens").GetProperty("accessToken").GetString();

            // Name
            name.ShouldNotBeNullOrEmpty();
            name.ShouldBe(_name);

            // Tokens
            accessToken.ShouldNotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Invalid_Login(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();

            var response = await DoPost(method: METHOD, request: request, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var errors = await GetErrorList(response);

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture))!;
            
            errors.ShouldHaveSingleItem();
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Empty_Is_Returning_Correct_Error(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();
            request.Email = "";

            var response = await DoPost(method: METHOD, request: request, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture))!;

            errors.ShouldHaveSingleItem();
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Invalid_Is_Returning_Correct_Error(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();
            request.Email = "email.com";

            var response = await DoPost(method: METHOD, request: request, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_INVALID", new CultureInfo(culture))!;

            errors.ShouldHaveSingleItem();
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Password_Empty_Is_Returning_Correct_Error(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();
            request.Password = "";

            var response = await DoPost(method: METHOD, request: request, culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            
            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture))!;

            errors.ShouldHaveSingleItem();
            errors.First().ToString().ShouldBe(expectedMessage);
        }
    }
}
