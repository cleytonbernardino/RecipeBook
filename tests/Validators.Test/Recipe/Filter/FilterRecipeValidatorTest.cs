using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe;
using RecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.Recipe.Filter
{
    public class FilterRecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_Invalid_Cooking_Time()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes[0] = (RecipeBook.Communication.Enums.CookingTime)1000;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_Invalid_Difficulty()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.Difficulties[0] = (RecipeBook.Communication.Enums.Difficulty)1000;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.DIFFICULTY_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_Invalid_DishTypes()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.DishTypes[0] = (RecipeBook.Communication.Enums.DishType)1000;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
        }
    }
}
