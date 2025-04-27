using CommonTestUtilities.IdEncription;
using CommonTestUtilities.Tokens;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
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
        public async Task Success()
        {
            string url = METHOD + _encripetedRecipeId;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoGet(url, token);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        
        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Recipe_Not_Found(string culture)
        {
            string encripetedRecipeId = IdEncripterBuilder.Build().Encode(1000);

            string url = METHOD + encripetedRecipeId;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoGet(url, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));
            errors.First().ToString().ShouldBe(expectedMessage);
        }
    }
}
