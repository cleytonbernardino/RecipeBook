using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using RecipeBook.Communiction.Requests;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update
{
    public class UpdateUserTest : RecipeBookClassFixture
    {
        private const string METHOD = "user";

        private readonly Guid _userIndentifier;

        public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIndentifier = factory.GetUserIndentifier();
        }

        [Fact]
        public async Task Success()
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();

            HttpResponseMessage response = await DoPut(METHOD, request, token);

            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Name = "";

            HttpResponseMessage response = await DoPut(METHOD, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            JsonElement jsonElement = await GetJsonElementAsync(response);
            var errors = jsonElement.GetProperty("errors").EnumerateArray();

            Assert.Single(errors);

            string expected = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture))!;
            Assert.Equal(expected, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Email_Empty(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "";

            HttpResponseMessage response = await DoPut(METHOD, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, ((int)response.StatusCode));

            JsonElement jsonElement = await GetJsonElementAsync(response);
            var errors = jsonElement.GetProperty("errors").EnumerateArray();

            Assert.Single(errors);
            string expected = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture))!;
            Assert.Equal(expected, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Email_Invalid(string culture)
        {
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "email.com";

            HttpResponseMessage response = await DoPut(METHOD, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, ((int)response.StatusCode));

            JsonElement jsonElement = await GetJsonElementAsync(response);
            var errors = jsonElement.GetProperty("errors").EnumerateArray();

            Assert.Single(errors);
            string expected = ResourceMessagesException.ResourceManager.GetString("EMAIL_INVALID", new CultureInfo(culture))!;
            Assert.Equal(expected, errors.First().ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Email_Already_In_Use(string culture)
        {
            // Creating another user to use the same email
            RequestRegisterUserJson userRequest = RequestUserJsonBuilder.Build();
            await DoPost(METHOD, userRequest);
            
            string token = JwtTokenGeneratorBuilder.Build().Generate(_userIndentifier);
            
            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Email = userRequest.Email;

            HttpResponseMessage response = await DoPut(METHOD, request, token, culture);

            Assert.Equal(StatusCodes.Status400BadRequest, ((int)response.StatusCode));

            JsonElement jsonElement = await GetJsonElementAsync(response);
            var errors = jsonElement.GetProperty("errors").EnumerateArray();

            Assert.Single(errors);
            string expected = ResourceMessagesException.ResourceManager.GetString("EMAIL_IN_USE", new CultureInfo(culture))!;
            Assert.Equal(expected, errors.First().ToString());
        }
    }
}
