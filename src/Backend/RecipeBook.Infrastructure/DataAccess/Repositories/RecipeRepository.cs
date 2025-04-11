using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;

namespace RecipeBook.Infrastructure.DataAccess.Repositories
{
    public class RecipeRepository : IRecipeReadOnlyRepository, IRecipeWriteOnlyRepository
    {
        private readonly RecipeBookDbContext _dbContext;

        public RecipeRepository(RecipeBookDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);

        public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
        {
            IQueryable<Recipe> query = _dbContext
                    .Recipes
                    .Include(recipe => recipe.Ingredients)
                    .AsNoTracking()
                    .Where(recipe => recipe.UserId == user.ID && user.Active);

            if (filters.Difficulties.Any())
                query = query.Where(recipe => recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));

            if (filters.CookingTimes.Any())
                query = query.Where(recipe => recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));

            if (filters.DishTypes.Any())
                query = query.Where(recipe => recipe.DishTypes.Any(dishType => filters.DishTypes.Contains(dishType.Type)));

            if (!string.IsNullOrEmpty(filters.RecipeTitle_Ingredient))
                query = query.Where(recipe => recipe.Title.Contains(filters.RecipeTitle_Ingredient)
                            || recipe.Ingredients.Any(ingredient => ingredient.Name.Contains(filters.RecipeTitle_Ingredient))
                        );

            return await query.ToListAsync();
        }
    }
}
