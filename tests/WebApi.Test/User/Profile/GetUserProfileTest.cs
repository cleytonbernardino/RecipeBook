using CommonTestUtilities.Tokens;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebApi.Test.User.Profile
{

    public class GetUserProfileTest : RecipeBookClassFixture
    {
        private const string METHOD = "user";

        private readonly string _name;
        private readonly string _email;
        private readonly Guid _userIndetifier;

        public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _name = factory.GetName();
            _email = factory.GetEmail();
            _userIndetifier = factory.GetUserIndentifier();
        }

        [Fact]
        public async Task Success()
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndetifier);
            HttpResponseMessage response = await DoGet(METHOD, token);

            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);

            JsonElement jsonElement = await GetJsonElementAsync(response);

            // Name
            var name = jsonElement.GetProperty("name").GetString();
            Assert.NotNull(name);
            Assert.NotEmpty(name!);
            Assert.Equal(_name, name);

            // Email
            var email = jsonElement.GetProperty("email").GetString();
            Assert.NotNull(email);
            Assert.NotEmpty(email!);
            Assert.Equal(_email, email);
        }
    }
}
