using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UserCases.Recipe.Generate
{
    public interface IGenerateRecipeUseCase
    {
        public Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request);
    }
}
