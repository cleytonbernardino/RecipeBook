using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
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
            
            var response = await DoGet(METHOD, token);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            JsonElement jsonElement = await GetJsonElementAsync(response);

            // Name
            var name = jsonElement.GetProperty("name").GetString();
            name.ShouldNotBeNullOrEmpty();
            name.ShouldBe(_name);

            // Email
            var email = jsonElement.GetProperty("email").GetString();
            email.ShouldNotBeNullOrEmpty();
            email.ShouldBe(_email);
        }
    }
}
