using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Filter
{
    public class GenerateRecipeInvalidToken : RecipeBookClassFixture
    {
        private const string METHOD = "recipe/filter";

        private readonly Guid _userIndentifier;

        public GenerateRecipeInvalidToken(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Invalid_Token(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPost(METHOD, request, token: "TokenInvalid", culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PERMISSION", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Without_Token(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPost(METHOD, request, token: "", culture: culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Without_User(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();

            string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
            
            var response = await DoPost(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var errors = await GetErrorList(response);
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PERMISSION", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Expired(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();

            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier, true);

            var response = await DoPost(METHOD, request, token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var jsonElement = await GetJsonElementAsync(response);

            // MsgError
            var errors = jsonElement.GetProperty("errors").EnumerateArray();
            errors.ShouldHaveSingleItem();

            string expectedMessage = ResourceMessagesException.ResourceManager.GetString("EXPIRED_TOKEN", new CultureInfo(culture))!;
            errors.First().ToString().ShouldBe(expectedMessage);

            // Status Expired
            bool tokenIsExpired = jsonElement.GetProperty("tokenIsExpired").GetBoolean();
            tokenIsExpired.ShouldBeTrue();
        }
    }
}
