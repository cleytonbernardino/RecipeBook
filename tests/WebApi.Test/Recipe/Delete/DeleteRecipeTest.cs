using CommonTestUtilities.IdEncription;
using CommonTestUtilities.Tokens;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Delete
{
    public class DeleteRecipeTest : RecipeBookClassFixture
    {
        private const string METHOD = "recipe/";

        private readonly Guid _userIntentifier;
        private readonly string _encriptedRecipeId;

        public DeleteRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIntentifier = factory.GetUserIndentifier();
            _encriptedRecipeId = factory.GetRecipeEncriptedId();
        }

        [Fact]
        public async Task Success()
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIntentifier);

            string url = METHOD + _encriptedRecipeId;

            var response = await DoDelete(url, token);
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            response = await DoGet(url, token);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Recipe_Not_Found(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIntentifier);

            string encriptedRecipeId = IdEncripterBuilder.Build().Encode(1000);
            string url = METHOD + encriptedRecipeId;

            var response = await DoDelete(url, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture))!;

            errors.First().ToString().ShouldBe(expectedMessage);
        }
    }
}
