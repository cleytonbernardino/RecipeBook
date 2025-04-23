using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Microsoft.AspNetCore.Http;
using RecipeBook.Communication.Enums;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using System.Globalization;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Update
{
    public class UpdateRecipeTest : RecipeBookClassFixture
    {
        private const string METHOD = "recipe/";

        private readonly Guid _userIndentifier;

        private readonly string _recipeId;

        public UpdateRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
            _recipeId = factory.GetRecipeEncriptedId();
        }

        [Fact]
        public async Task Success()
        {
            string url = METHOD + _recipeId;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPut(url, request, token);

            Assert.Equal(StatusCodes.Status204NoContent, ((int)response.StatusCode));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Title_Empty(string culture)
        {
            string url = METHOD + _recipeId;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = "";

            var response = await DoPut(url, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, ((int)response.StatusCode));

            var errors = await GetErrorList(response);

            Assert.Single(errors);
            
            string expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("TITLE_EMPTY", new CultureInfo(culture))!;
            Assert.Equal(expectedErrorMessage, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Cooking_Time_Out_Range(string culture)
        {
            string url = METHOD + _recipeId;

            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = (CookingTime)1000;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPut(url, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            var errors = await GetErrorList(response);

            Assert.Single(errors);

            string expectedMsg = ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture))!;
            Assert.Equal(expectedMsg, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Difficulty_Out_Range(string culture)
        {
            string url = METHOD + _recipeId;

            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = (Difficulty)1000;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPut(url, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            var errors = await GetErrorList(response);

            Assert.Single(errors);

            string expectedMsg = ResourceMessagesException.ResourceManager.GetString("DIFFICULTY_NOT_SUPPORTED", new CultureInfo(culture))!;
            Assert.Equal(expectedMsg, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Ingredients_Cannot_Be_Empty(string culture)
        {
            string url = METHOD + _recipeId;

            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Clear();

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            HttpResponseMessage response = await DoPut(url, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            var errors = await GetErrorList(response);

            Assert.Single(errors);

            string expectedMsg = ResourceMessagesException.ResourceManager.GetString("MUST_CONTAIN_ONE_INGREDIENT", new CultureInfo(culture))!;
            Assert.Equal(expectedMsg, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Ingredients_Invalid_Type(string culture)
        {
            string url = METHOD + _recipeId;

            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients[0] = null;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            HttpResponseMessage response = await DoPut(url, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            var errors = await GetErrorList(response);

            Assert.Single(errors);

            string expectedMsg = ResourceMessagesException.ResourceManager.GetString("INGREDIENT_EMPTY", new CultureInfo(culture))!;
            Assert.Equal(expectedMsg, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Instructions_Cannot_Be_Empty(string culture)
        {
            string url = METHOD + _recipeId;

            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.Clear();

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            HttpResponseMessage response = await DoPut(url, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            var errors = await GetErrorList(response);

            Assert.Single(errors);

            string expectedMsg = ResourceMessagesException.ResourceManager.GetString("MUST_CONTAIN_AN_INSTRUCTION", new CultureInfo(culture))!;
            Assert.Equal(expectedMsg, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Same_Instructions(string culture)
        {
            string url = METHOD + _recipeId;

            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            HttpResponseMessage response = await DoPut(url, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, ((int)response.StatusCode));

            var errors = await GetErrorList(response);

            Assert.Single(errors);

            string expectedMenssage = ResourceMessagesException.ResourceManager.GetString("TWO_OR_MORE_INSTRUCTIONS_HAVE_THE_SAME_ORDER", new CultureInfo(culture))!;

            Assert.Equal(expectedMenssage, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Negative_Step_Instructions(string culture)
        {
            string url = METHOD + _recipeId;

            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions[0].Step = -1;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            HttpResponseMessage response = await DoPut(url, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, ((int)response.StatusCode));

            var errors = await GetErrorList(response);
            Assert.Single(errors);

            string expectedMenssage = ResourceMessagesException.ResourceManager.GetString("NO_NEGATIVE_INSTRUCTION_STEP", new CultureInfo(culture))!;
            Assert.Equal(expectedMenssage, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_DishTypes_Out_Range(string culture)
        {
            string url = METHOD + _recipeId;

            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Add((DishType)1000);

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            HttpResponseMessage response = await DoPut(url, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            var errors = await GetErrorList(response);

            Assert.Single(errors);

            string expectedMsg = ResourceMessagesException.ResourceManager.GetString("DISH_TYPE_NOT_SUPPORTED", new CultureInfo(culture))!;
            Assert.Equal(expectedMsg, errors.First().ToString());
        }
    }
}
