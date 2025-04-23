using RecipeBook.Domain.Dtos;

namespace RecipeBook.Domain.Repositories.Recipe
{
    public interface IRecipeReadOnlyRepository
    {
        public Task<IList<Entities.Recipe>> Filter(Entities.User user, FilterRecipesDto filters);
        public Task<Entities.Recipe?> GetById(Entities.User user, long recipeId);
        public Task<bool> IsValidRecipeOwner(Entities.User user, long recipeId);
        public Task<IList<Entities.Recipe>> GetForDashbord(Entities.User user);
    }
}
