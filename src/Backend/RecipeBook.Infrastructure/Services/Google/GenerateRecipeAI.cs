using Mscc.GenerativeAI;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Services.OpenAI;

namespace RecipeBook.Infrastructure.Services.Google;

public class GenerateRecipeAI : IGenerateRecipeAI
{
    private readonly IGenerativeAI _generativeAI;

    public GenerateRecipeAI(IGenerativeAI generativeAI)
    {
        _generativeAI = generativeAI;
    }

    public async Task<GenerateRecipeDto> Generate(IList<string> ingredients)
    {
        var generativeModel = _generativeAI.GenerativeModel(model: Model.Gemini25FlashPreview0417);

        var chat = generativeModel.StartChat();

        string ingredientString = string.Join(";", ingredients);
        string prompt = $"{ResourceGeminiAi.STARTING_GENERATE_RECIPE}\n\n{ResourceGeminiAi.INGREDIENTS_PROVIDED_LEAD_IN}{ingredientString}";

        await chat.SendMessage(prompt);

        var result = chat.Last!.Text;

        var recipeItems = result
            .Split("\n")
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Select(item => item.Replace("[", "").Replace("]", ""))
            .ToList();

        int step = 1;

        return new GenerateRecipeDto()
        {
            Title = recipeItems[0],
            CookingTime = (CookingTime)Enum.Parse(typeof(CookingTime), recipeItems[1]),
            Ingredients = recipeItems[2].Split(";"),
            Instructions = recipeItems[3].Split("@").Select(instruction => new GenerateInstructionDto
            {
                Step = step++,
                Text = instruction.Trim()
            }).ToList()
        };
    }
}
