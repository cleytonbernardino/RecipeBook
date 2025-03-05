using RecipeBook.Communiction.Requests;

namespace RecipeBook.Application.UserCases.User.Update
{
    public interface IUpdateUserProfile
    {
        public Task Execute(RequestUpdateUserJson request);
    }
}
