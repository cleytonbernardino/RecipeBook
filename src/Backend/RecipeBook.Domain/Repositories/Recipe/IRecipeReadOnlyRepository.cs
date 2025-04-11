using RecipeBook.Domain.Dtos;

namespace RecipeBook.Domain.Repositories.Recipe
{
    public interface IRecipeReadOnlyRepository
    {
        Task<IList<Entities.Recipe>> Filter(Entities.User user, FilterRecipesDto filters);
    }
}
