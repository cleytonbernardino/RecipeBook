using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UserCases.User.Update
{
    public interface IUpdateUserUseCase
    {
        public Task Execute(RequestUpdateUserJson request);
    }
}
