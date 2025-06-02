using RecipeBook.Domain.Enums;

namespace RecipeBook.Domain.Dtos
{
    public class GenerateRecipeDto
    {
        public string Title { get; init; } = "";
        public IList<string> Ingredients { get; init; } = [];
        public IList<GenerateInstructionDto> Instructions { get; init; } = [];
        public CookingTime CookingTime { get; init; }
    }
}
