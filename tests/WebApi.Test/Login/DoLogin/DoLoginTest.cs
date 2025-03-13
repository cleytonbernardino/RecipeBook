using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : RecipeBookClassFixture
    {
        private readonly string _method = "login";

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

            HttpResponseMessage response = await DoPost(_method, request);

            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            string? name = responseData.RootElement.GetProperty("name").GetString();

            string? accessToken = responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString();

            // Name
            Assert.NotNull(name);
            Assert.NotEmpty(name);
            Assert.Equal(_name, name);

            // Tokens
            Assert.NotNull(accessToken);
            Assert.NotEmpty(accessToken);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Invalid_Login(string culture)
        {
            RequestLoginJson request = RequestLoginJsonBuilder.Build();

            HttpResponseMessage response = await DoPost(_method, request, culture);

            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            JsonElement jsonData = responseData.RootElement.GetProperty("errors");

            Assert.Equal(1, jsonData.GetArrayLength());

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture))!;

            Assert.Equal(expectedMessage, jsonData[0].GetString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Empty_Is_Returning_Correct_Error(string culture)
        {
            RequestLoginJson request = RequestLoginJsonBuilder.Build();
            request.Email = "";

            HttpResponseMessage response = await DoPost(_method, request, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            JsonElement jsonData = responseData.RootElement.GetProperty("errors");

            Assert.Equal(1, jsonData.GetArrayLength());

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture))!;

            Assert.Equal(expectedMessage, jsonData[0].GetString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Invalid_Is_Returning_Correct_Error(string culture)
        {
            RequestLoginJson request = RequestLoginJsonBuilder.Build();
            request.Email = "email.com";

            HttpResponseMessage response = await DoPost(_method, request, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            JsonElement jsonData = responseData.RootElement.GetProperty("errors");

            Assert.Equal(1, jsonData.GetArrayLength());

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_INVALID", new CultureInfo(culture))!;

            Assert.Equal(expectedMessage, jsonData[0].GetString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Password_Empty_Is_Returning_Correct_Error(string culture)
        {
            RequestLoginJson request = RequestLoginJsonBuilder.Build();
            request.Password = "";

            HttpResponseMessage response = await DoPost(_method, request, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            JsonElement jsonData = responseData.RootElement.GetProperty("errors");

            Assert.Equal(1, jsonData.GetArrayLength());

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture))!;

            Assert.Equal(expectedMessage, jsonData[0].GetString());
        }
    }
}
