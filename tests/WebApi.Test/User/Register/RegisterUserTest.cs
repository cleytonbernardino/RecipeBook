using CommonTestUtilities.Requests;
using RecipeBook.Communiction.Requests;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly string _method = "user";

        private readonly HttpClient _httpClient;

        public RegisterUserTest(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

        [Fact]
        public async Task Success()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_method, request);

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

            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_method, request);

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

            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_method, request);

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

            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_method, request);

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

            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

            await _httpClient.PostAsJsonAsync(_method, request);
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_method, request);

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

            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_method, request);

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
