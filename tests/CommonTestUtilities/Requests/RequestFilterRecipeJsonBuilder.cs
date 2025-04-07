using Bogus;
using RecipeBook.Communication.Enums;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestFilterRecipeJsonBuilder
    {
        public static RequestFilterRecipeJson Build()
        {
            return new Faker<RequestFilterRecipeJson>()
                .RuleFor(recipe => recipe.CookingTimes, f => f.Make(1, () => f.PickRandom<CookingTime>()))
                .RuleFor(recipe => recipe.Difficulties, f => f.Make(1, () => f.PickRandom<Difficulty>()))
                .RuleFor(recipe => recipe.DishTypes, f => f.Make(1, () => f.PickRandom<DishType>()))
                .RuleFor(recipe => recipe.RecipeTitle_Ingredient, f => f.Lorem.Word())
            ;
        }
    }
}
