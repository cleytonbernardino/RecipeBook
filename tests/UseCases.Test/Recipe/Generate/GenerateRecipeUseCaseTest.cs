using CommonTestUtilities.Dtos;
using CommonTestUtilities.Google;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe.Generate;
using RecipeBook.Communication.Enums;
using RecipeBook.Domain.Dtos;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Generate;

public class GenerateRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var dto = GenerateRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(dto);
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(dto.Title);
        result.CookingTime.ShouldBe((CookingTime)dto.CookingTime);
        result.Difficulty.ShouldBe(Difficulty.Easy);
    }

    [Fact]
    private async Task Error_Duplicated_Ingredients()
    {
        var dto = GenerateRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeJsonBuilder.Build(3);
        request.Ingredients[0] = request.Ingredients[1];

        var useCase = CreateUseCase(dto);

        async Task act() => await useCase.Execute(request);

        var result = await act().ShouldThrowAsync<ErrorOnValidationException>();

        result.ErrorMessagens.Single().ShouldBe(ResourceMessagesException.DUPLICATE_INGREDIENT_IN_LIST);
    }

    private static GenerateRecipeUseCase CreateUseCase(GenerateRecipeDto dto)
    {
        var generateAI = GenerateRecipeAIBuilder.Build(dto);

        return new GenerateRecipeUseCase(generateAI);
    }
}
