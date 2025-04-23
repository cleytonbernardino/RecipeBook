namespace RecipeBook.Domain.Repositories.Recipe
{
    public interface IRecipeUpdateOnlyRepository
    {
        public Task<Entities.Recipe?> GetById(Entities.User user, long id);

        public void Update(Entities.Recipe recipe);
    }
}
