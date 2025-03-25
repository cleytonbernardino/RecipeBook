using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UserCases.User.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        public Task Execute(RequestChangePasswordJson request);
    }
}
