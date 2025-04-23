using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UserCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseResgisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}
