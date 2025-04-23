using CommonTestUtilities.IdEncription;
using CommonTestUtilities.Tokens;
using Microsoft.AspNetCore.Http;
using RecipeBook.Exceptions;
using System.Globalization;
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

            Assert.Equal(StatusCodes.Status204NoContent, ((int)response.StatusCode));

            response = await DoGet(url, token);

            Assert.Equal(StatusCodes.Status404NotFound, ((int)response.StatusCode));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Recipe_Not_Found(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIntentifier);

            string encriptedRecipeId = IdEncripterBuilder.Build().Encode(1000);
            string url = METHOD + encriptedRecipeId;

            var response = await DoDelete(url, token, culture);

            Assert.Equal(StatusCodes.Status404NotFound, ((int)response.StatusCode));

            var errors = await GetErrorList(response);
            Assert.Single(errors);

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture))!;
            Assert.Equal(expectedMessage, errors.First().ToString());
        }
    }
}
