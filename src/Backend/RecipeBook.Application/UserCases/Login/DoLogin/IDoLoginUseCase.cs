using RecipeBook.Communiction.Requests;
using RecipeBook.Communiction.Responses;

namespace RecipeBook.Application.UserCases.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        public Task<ResponseResgisteredUserJson> Execute(RequestLoginJson request);
    }
}
