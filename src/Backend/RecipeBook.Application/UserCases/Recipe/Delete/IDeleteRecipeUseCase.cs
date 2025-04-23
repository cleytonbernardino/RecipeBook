namespace RecipeBook.Application.UserCases.Recipe.Delete
{
    public interface IDeleteRecipeUseCase
    {
        public Task Execute(long recipeId);
    }
}
