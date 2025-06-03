using Bogus;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Dtos;

namespace CommonTestUtilities.Dtos;

public class GenerateRecipeDtoBuilder
{
    public static GenerateRecipeDto Build()
    {
        return new Faker<GenerateRecipeDto>()
            .RuleFor(recipe => recipe.Title, f => f.Lorem.Word())
            .RuleFor(recipe => recipe.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(recipe => recipe.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
            .RuleFor(recipe => recipe.Instructions, f => f.Make(1, () => new GenerateInstructionDto
            {
                Step = 1,
                Text = f.Lorem.Paragraph()
            }));
    }
}
