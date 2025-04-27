using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Enums;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Filter
{
    public class FilterRecipeTest : RecipeBookClassFixture
    {
        private const string METHOD = "recipe/filter";

        private readonly Guid _userIndentifier;

        private readonly string _recipeTile;
        private readonly Difficulty _difficulty;
        private readonly CookingTime _cookingTime;
        private readonly IList<DishType> _dishTypes;

        public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
            _recipeTile = factory.GetRecipeTitle();
            _difficulty = factory.GetRecipeDifficulty();
            _cookingTime = factory.GetRecipeCookingTIme();
            _dishTypes = factory.GetDishTypes();
        }

        [Fact]
        public async Task Success()
        {
            RequestFilterRecipeJson request = new()
            {
                RecipeTitle_Ingredient = _recipeTile,
                Difficulties = [(RecipeBook.Communication.Enums.Difficulty)_difficulty],
                CookingTimes = [(RecipeBook.Communication.Enums.CookingTime)_cookingTime],
                DishTypes = _dishTypes.Select(dishType => (RecipeBook.Communication.Enums.DishType)dishType).ToList()
            };

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPost(METHOD, request, token);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Success_NoContent()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPost(METHOD, request, token);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_CookingTime_Invalid(string culture)
        {
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((RecipeBook.Communication.Enums.CookingTime)1000);

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPost(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string errorMessage = ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(errorMessage);
        }
    }
}
