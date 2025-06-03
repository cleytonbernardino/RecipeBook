using Bogus;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestGenerateRecipeJsonBuilder
{
    public static RequestGenerateRecipeJson Build(int count = 5)
    {
        return new Faker<RequestGenerateRecipeJson>()
            .RuleFor(recipe => recipe.Ingredients, f => f.Make(count, () => f.Commerce.ProductName()));
    }
}
