using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UserCases.User.Profile
{
    public interface IGetUserProfile
    {
        public Task<ResponseUserProfileJson> Execute();
    }
}
