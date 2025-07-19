using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Communication.Enums;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Register
{
    public class RegisterRecipeTest : RecipeBookClassFixture
    {
        private const string METHOD = "recipe";

        private readonly Guid _userIndentifier;
        public RegisterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Title = "";

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMsg = ResourceMessagesException.ResourceManager.GetString("TITLE_EMPTY", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMsg);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Cooking_Time_Out_Range(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.CookingTime = (CookingTime)1000;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Difficulty_Out_Range(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Difficulty = (Difficulty)1000;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("DIFFICULTY_NOT_SUPPORTED", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Ingredients_Cannot_Be_Empty(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Ingredients.Clear();

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("MUST_CONTAIN_ONE_INGREDIENT", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Ingredients_Invalid_Type(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Ingredients[0] = "";

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("INGREDIENT_EMPTY", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Instructions_Cannot_Be_Empty(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Instructions.Clear();

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("MUST_CONTAIN_AN_INSTRUCTION", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Same_Instructions(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMenssage = ResourceMessagesException.ResourceManager.GetString("TWO_OR_MORE_INSTRUCTIONS_HAVE_THE_SAME_ORDER", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMenssage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Negative_Step_Instructions(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Instructions[0].Step = -1;

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMenssage = ResourceMessagesException.ResourceManager.GetString("NO_NEGATIVE_INSTRUCTION_STEP", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMenssage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_DishTypes_Out_Range(string culture)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.DishTypes.Add((DishType)1000);

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            var response = await DoPostFormData(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("DISH_TYPE_NOT_SUPPORTED", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

    }
}
