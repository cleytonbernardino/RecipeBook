using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UserCases.Recipe.GetById
{
    public interface IGetRecipeByIdUseCase
    {
        Task<ResponseRecipeJson> Execute(long recipeId);
    }
}
