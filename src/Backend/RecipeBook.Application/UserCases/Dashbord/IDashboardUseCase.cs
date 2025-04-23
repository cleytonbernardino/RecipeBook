using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UserCases.Dashbord
{
    public interface IDashboardUseCase
    {
        public Task<ResponsesRecipesJson> Execute();
    }
}
