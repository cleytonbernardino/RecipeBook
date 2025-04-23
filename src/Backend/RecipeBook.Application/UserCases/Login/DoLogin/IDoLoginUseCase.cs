using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UserCases.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        public Task<ResponseResgisteredUserJson> Execute(RequestLoginJson request);
    }
}
