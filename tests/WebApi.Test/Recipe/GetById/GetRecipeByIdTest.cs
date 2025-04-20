using CommonTestUtilities.IdEncription;
using CommonTestUtilities.Tokens;
using Microsoft.AspNetCore.Http;
using RecipeBook.Exceptions;
using System.Globalization;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.GetById
{
    public class GetRecipeByIdTest : RecipeBookClassFixture
    {
        private const string METHOD = "recipe/";

        private readonly string _encripetedRecipeId;
        private readonly Guid _userIndentifier;

        public GetRecipeByIdTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _encripetedRecipeId = factory.GetRecipeEncriptedId();
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Fact]
        public async void Success()
        {
            string url = METHOD + _encripetedRecipeId;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoGet(url, token);

            Assert.Equal(StatusCodes.Status200OK, ((int)response.StatusCode));
        }
        
        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async void Error_Recipe_Not_Found(string culture)
        {
            string encripetedRecipeId = IdEncripterBuilder.Build().Encode(1000);

            string url = METHOD + encripetedRecipeId;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoGet(url, token, culture);
            Assert.Equal(StatusCodes.Status404NotFound, ((int)response.StatusCode));

            var errors = await GetErrorList(response);

            Assert.Single(errors);

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));
            Assert.Equal(expectedMessage, errors.First().ToString());
        }
    }
}
