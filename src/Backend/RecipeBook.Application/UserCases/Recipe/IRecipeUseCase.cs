using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UserCases.Recipe
{
    public interface IRecipeUseCase
    {
        public Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request);
    }
}
