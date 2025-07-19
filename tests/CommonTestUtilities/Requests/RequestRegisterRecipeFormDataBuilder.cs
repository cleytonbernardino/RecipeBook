using Bogus;
using Microsoft.AspNetCore.Http;
using RecipeBook.Communication.Enums;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterRecipeFormDataBuilder
{
    public static RequestRegisterRecipeFormData Build(IFormFile? formFile = null)
    {
        int step = 1;
        return new Faker<RequestRegisterRecipeFormData>()
            .RuleFor(recipe => recipe.Image, _ => formFile)
            .RuleFor(recipe => recipe.Title, f => f.Commerce.Product())
            .RuleFor(recipe => recipe.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(recipe => recipe.Difficulty, f => f.PickRandom<Difficulty>())
            .RuleFor(recipe => recipe.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
            .RuleFor(recipe => recipe.Instructions, f => f.Make(3, () => new RequestInstructionJson
            {
                Step = step++,
                Text = f.Lorem.Word()
            }))
            .RuleFor(recipe => recipe.DishTypes, f => f.Make(3, () => f.PickRandom<DishType>()));
    }
}
