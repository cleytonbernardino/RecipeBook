using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.Test.Dashboard
{
    public class DashboardTest : RecipeBookClassFixture
    {
        private const string METHOD = "dashboard";

        private readonly Guid _userIdentifier;

        public DashboardTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIndentifier();
        }

        [Fact]
        public async Task Success()
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoGet(METHOD, token);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var responseData = await GetJsonElementAsync(response);

            responseData.GetProperty("recipes").GetArrayLength().ShouldBeGreaterThan(0);
        }
    }
}
