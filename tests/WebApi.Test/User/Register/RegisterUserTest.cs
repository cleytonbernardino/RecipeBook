using CommonTestUtilities.Requests;
using RecipeBook.Communiction.Requests;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public RegisterUserTest(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

        [Fact]
        public async Task Success()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.MakeRequest();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("User", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            await using Stream responseBody = await response.Content.ReadAsStreamAsync();

            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);

            string? name = responseData.RootElement.GetProperty("name").GetString();

            Assert.Equal(request.Name, name);
        }
    }
}
