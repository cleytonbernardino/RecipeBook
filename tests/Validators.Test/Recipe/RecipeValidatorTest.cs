using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe;
using RecipeBook.Communication.Enums;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;

namespace Validators.Test.Recipe
{
    public class RecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Success_Cooking_Time_Null()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = null;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Success_Difficulty_Time_Null()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = null;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Success_DishTypes_Empty()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Clear();

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("    ")]
        [InlineData("")]
        public void Error_Title_Empty(string? title)
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Title = title!;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.TITLE_EMPTY, result.Errors[0].ToString());
        }

        [Fact]
        public void Error_Invalid_Cooking_time()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = (CookingTime)1000;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED, result.Errors[0].ToString());
        }

        [Fact]
        public void Error_Invalid_Difficulty()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = (Difficulty)1000;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.DIFFICULTY_NOT_SUPPORTED, result.Errors[0].ToString());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("    ")]
        [InlineData("")]
        public void Error_Empty_Value_Ingredients(string? ingredients)
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Add(ingredients!);

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.INGREDIENT_EMPTY, result.Errors[0].ToString());

        }

        [Fact]
        public void Error_Empty_Instructions()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Instructions.Clear();

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.MUST_CONTAIN_AN_INSTRUCTION, result.Errors[0].ToString());
        }


        [Fact]
        public void Error_Same_Instructions()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_HAVE_THE_SAME_ORDER, result.Errors[0].ToString());
        }

        [Fact]
        public void Error_Negative_Step_Instructions()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Instructions[0].Step = -1;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.NO_NEGATIVE_INSTRUCTION_STEP, result.Errors[0].ToString());
        }

        [Theory]
        [InlineData("    ")]
        [InlineData("")]
        public void Error_Empty_Value_Instructions(string instructions)
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Instructions[0].Text = instructions;

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.INSTRUCTION_EMPTY, result.Errors[0].ToString());
        }

        [Fact]
        public void Error_Invalid_DishTypes()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Add((DishType)1000);

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED, result.Errors[0].ToString());
        }

        [Fact]
        public void Error_Instructions_Too_Long()
        {
            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Instructions[0].Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);

            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.INSTRUCTION_EXCEEDS_MAXIMUM_SIZE, result.Errors[0].ToString());
        }
    }
}
