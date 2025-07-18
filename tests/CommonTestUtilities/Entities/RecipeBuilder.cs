using Bogus;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class RecipeBuilder
    {
        public static IList<Recipe> Collection(User user, uint count = 2)
        {
            if (count == 0)
                count = 2;

            List<Recipe> recipes = [];
            Recipe recipe;

            for (int i=0; i < count; i++)
            {
                recipe = Build(user);
                recipe.ID = i + 1;
                recipes.Add(recipe);
            }
            return recipes;
        }

        public static Recipe Build(User user)
        {
            return new Faker<Recipe>()
                .RuleFor(recipe => recipe.ID, () => 1)
                .RuleFor(recipe => recipe.UserId, () => user.ID)
                .RuleFor(recipe => recipe.Title, f => f.Lorem.Word())
                .RuleFor(recipe => recipe.CookingTime, f => f.PickRandom<CookingTime>())
                .RuleFor(recipe => recipe.Difficulty, f => f.PickRandom<Difficulty>())
                .RuleFor(recipe => recipe.ImageIndentifier, _ => $"{Guid.NewGuid()}.jpg")
                .RuleFor(recipe => recipe.Ingredients, f => f.Make(1, () => new Ingredient()
                {
                    ID = 1,
                    Name = f.Commerce.ProductName()
                }))
                .RuleFor(recipe => recipe.Instructions, f => f.Make(1, () => new Instruction()
                {
                    ID = 1,
                    Step = 1,
                    Text = f.Lorem.Paragraph()
                }))
                .RuleFor(recipe => recipe.DishTypes, f => f.Make(1, () => new RecipeBook.Domain.Entities.DishType()
                {
                    ID = 1,
                    Type = f.PickRandom<RecipeBook.Domain.Enums.DishType>()
                }))
            ;
        }
    }
}
