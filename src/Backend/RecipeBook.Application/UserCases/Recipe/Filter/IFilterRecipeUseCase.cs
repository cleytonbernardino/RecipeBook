using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UserCases.Recipe.Filter
{
    public interface IFilterRecipeUseCase
    {
        public Task<ResponsesRecipesJson> Execute(RequestFilterRecipeJson request);
    }
}
