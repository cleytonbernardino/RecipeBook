using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Services.OpenAI;

namespace RecipeBook.Infrastructure.Services.OpenAI
{
    public class GenerateRecipeAI : IGenerateRecipeAI
    {
        private const string CHAT_MODEL = "gpt-4o mini";

        public async Task<GenerateRecipeDto> Generate(IList<string> ingredients)
        {
            throw new NotImplementedException();
        }
    }
}
