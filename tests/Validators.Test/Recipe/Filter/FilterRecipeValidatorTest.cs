using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;

namespace Validators.Test.Recipe.Filter
{
    public class FilterRecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            RequestFilterRecipeJson request = RequestFilterRecipeJsonBuilder.Build();

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_Invalid_Cooking_Time()
        {
            RequestFilterRecipeJson request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes[0] = (RecipeBook.Communication.Enums.CookingTime)1000;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED, result.Errors[0].ToString());
        }

        [Fact]
        public void Error_Invalid_Difficulty()
        {
            RequestFilterRecipeJson request = RequestFilterRecipeJsonBuilder.Build();
            request.Difficulties[0] = (RecipeBook.Communication.Enums.Difficulty)1000;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.DIFFICULTY_NOT_SUPPORTED, result.Errors[0].ToString());
        }

        [Fact]
        public void Error_Invalid_DishTypes()
        {
            RequestFilterRecipeJson request = RequestFilterRecipeJsonBuilder.Build();
            request.DishTypes[0] = (RecipeBook.Communication.Enums.DishType)1000;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED, result.Errors[0].ToString());
        }
    }
}
