using CommonTestUtilities.Requests;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
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
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();

            HttpResponseMessage response = await DoPost(METHOD, request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            string? name = responseData.RootElement.GetProperty("name").GetString();

            Assert.Equal(request.Name, name);
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Name_Empty_Is_Returning_Correct_Error(string culture)
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            request.Name = "";

            HttpResponseMessage response = await DoPost(method: METHOD, request: request, culture: culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            JsonElement jsonData = responseData.RootElement.GetProperty("errors");

            Assert.Equal(1, jsonData.GetArrayLength());

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture))!;

            Assert.Equal(expectedMessage, jsonData[0].GetString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Empty_Is_Returning_Correct_Error(string culture)
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            request.Email = string.Empty;

            HttpResponseMessage response = await DoPost(method: METHOD, request: request, culture: culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            JsonElement jsonData = responseData.RootElement.GetProperty("errors");

            Assert.Equal(1, jsonData.GetArrayLength());

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture))!;

            Assert.Equal(expectedMessage, jsonData[0].GetString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Invalid_Is_Returning_Correct_Error(string culture)
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            request.Email = "email.com";

            HttpResponseMessage response = await DoPost(method: METHOD, request: request, culture: culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            JsonElement jsonData = responseData.RootElement.GetProperty("errors");

            Assert.Equal(1, jsonData.GetArrayLength());

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_INVALID", new CultureInfo(culture))!;

            Assert.Equal(expectedMessage, jsonData[0].GetString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Email_Already_Exists_Is_Returning_Correct_Error(string culture)
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();

            await DoPost(method: METHOD, request: request);
            HttpResponseMessage response = await DoPost(method: METHOD, request: request, culture: culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            JsonElement jsonData = responseData.RootElement.GetProperty("errors");

            Assert.Equal(1, jsonData.GetArrayLength());

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_IN_USE", new CultureInfo(culture))!;

            Assert.Equal(expectedMessage, jsonData[0].GetString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Password_Lenght_Is_Returning_Correct_Error(string culture)
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            request.Password = "123";

            HttpResponseMessage response = await DoPost(method: METHOD, request: request, culture: culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            JsonElement jsonData = responseData.RootElement.GetProperty("errors");

            Assert.Equal(1, jsonData.GetArrayLength());

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_LENGTH_INVALID", new CultureInfo(culture))!;

            Assert.Equal(expectedMessage, jsonData[0].GetString());
        }
    }
}
