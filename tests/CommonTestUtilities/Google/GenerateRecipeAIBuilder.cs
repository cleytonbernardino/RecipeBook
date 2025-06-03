using Moq;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Services.OpenAI;

namespace CommonTestUtilities.Google;

public class GenerateRecipeAIBuilder
{
    public static IGenerateRecipeAI Build(GenerateRecipeDto dto)
    {
        var mock = new Mock<IGenerateRecipeAI>();

        mock.Setup(service => service.Generate(It.IsAny<List<string>>())).ReturnsAsync(dto);

        return mock.Object;
    }
}
