using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe.Register;
using RecipeBook.Communication.Enums;
using RecipeBook.Exceptions;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace Validators.Test.Recipe
{
    public class RecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestRecipeJsonBuilder.Build();

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Success_Cooking_Time_Null()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = null;

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Success_Difficulty_Time_Null()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = null;

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Success_DishTypes_Empty()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Clear();

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("    ")]
        [InlineData("")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Because is a unit test")]
        public void Error_Title_Empty(string title)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Title = title;

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.TITLE_EMPTY);
        }

        [Fact]
        public void Error_Invalid_Cooking_time()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = (CookingTime)1000;

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_Invalid_Difficulty()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = (Difficulty)1000;

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.DIFFICULTY_NOT_SUPPORTED);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("    ")]
        [InlineData("")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Because is a unit test")]
        public void Error_Empty_Value_Ingredients(string ingredients)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Add(ingredients);

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.INGREDIENT_EMPTY);
        }

        [Fact]
        public void Error_Empty_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.Clear();

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.MUST_CONTAIN_AN_INSTRUCTION);
        }


        [Fact]
        public void Error_Same_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_HAVE_THE_SAME_ORDER);
        }

        [Fact]
        public void Error_Negative_Step_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions[0].Step = -1;

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.NO_NEGATIVE_INSTRUCTION_STEP);
        }

        [Theory]
        [InlineData("    ")]
        [InlineData("")]
        [InlineData(null)]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Because is a unit test")]
        public void Error_Empty_Value_Instructions(string instructions)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions[0].Text = instructions;

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.INSTRUCTION_EMPTY);
        }

        [Fact]
        public void Error_Invalid_DishTypes()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Add((DishType)1000);

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_Instructions_Too_Long()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions[0].Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);

            RecipeValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.INSTRUCTION_EXCEEDS_MAXIMUM_SIZE);
        }
    }
}
