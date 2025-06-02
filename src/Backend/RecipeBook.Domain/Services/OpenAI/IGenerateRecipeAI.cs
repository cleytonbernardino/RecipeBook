
using RecipeBook.Domain.Dtos;

namespace RecipeBook.Domain.Services.OpenAI
{
    public interface IGenerateRecipeAI
    {
        Task<GenerateRecipeDto> Generate(IList<string> ingredients);
    }
}
