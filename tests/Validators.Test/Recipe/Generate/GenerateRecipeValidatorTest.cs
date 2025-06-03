using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe.Generate;
using RecipeBook.Domain.ValueObjects;
using RecipeBook.Exceptions;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace Validators.Test.Recipe.Generate;

public class GenerateRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new GenerateRecipeValidator();

        var request = RequestGenerateRecipeJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_More_Maximum_Ingredient()
    {
        var validator = new GenerateRecipeValidator();

        var request = RequestGenerateRecipeJsonBuilder.Build(RecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE + 1);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ErrorMessage.ShouldBe(ResourceMessagesException.INVALID_NUMBER_INGREDIENTS);
    }

    [Fact]
    public void Error_Duplicated_Ingredient()
    {
        var validator = new GenerateRecipeValidator();

        var request = RequestGenerateRecipeJsonBuilder.Build(RecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
        request.Ingredients.Add(request.Ingredients[0]);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ErrorMessage.ShouldBe(ResourceMessagesException.DUPLICATE_INGREDIENT_IN_LIST);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("       ")]
    [InlineData("")]    
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Because it is a unit test")]
    public void Erro_Empty_Ingredient(string ingredient)
    {
        var validator = new GenerateRecipeValidator();

        var request = RequestGenerateRecipeJsonBuilder.Build();
        request.Ingredients[0] = ingredient;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ErrorMessage.ShouldBe(ResourceMessagesException.INGREDIENT_EMPTY);
    }


    [Theory]
    [InlineData("This a invalid ingredient because is too long")]
    [InlineData("This/an/invalid/ingredient/because/contain/more/one/bar")]
    public void Error_Ingredient_Not_Following_Patern(string ingredient)
    {
        var validator = new GenerateRecipeValidator();

        var request = RequestGenerateRecipeJsonBuilder.Build();
        request.Ingredients[0] = ingredient;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ErrorMessage.ShouldBe(ResourceMessagesException.INGREDIENT_NOT_FOLLOWING_PATTERN);
    }
}
