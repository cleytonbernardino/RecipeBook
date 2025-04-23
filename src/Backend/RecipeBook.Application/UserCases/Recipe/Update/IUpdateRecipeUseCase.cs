using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UserCases.Recipe.Update
{
    public interface IUpdateRecipeUseCase
    {
        public Task Execute(long id, RequestRecipeJson request);
    }
}
