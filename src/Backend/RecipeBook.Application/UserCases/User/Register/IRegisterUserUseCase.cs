using RecipeBook.Communiction.Requests;
using RecipeBook.Communiction.Responses;

namespace RecipeBook.Application.UserCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseResgisteredUserJson> Execute(RequestRegisterUserJson request);
        //public void Validate(RequestRegisterUserJson request);
    }
}
