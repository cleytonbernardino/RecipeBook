using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;

namespace RecipeBook.Infrastructure.DataAccess.Repositories
{
    public class RecipeRepository : IRecipeWriteOnlyRepository
    {
        private readonly RecipeBookDbContext _dbContext;

        public RecipeRepository(RecipeBookDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);
    }
}
