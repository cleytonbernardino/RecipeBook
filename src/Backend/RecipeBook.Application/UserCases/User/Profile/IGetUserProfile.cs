using RecipeBook.Communiction.Responses;

namespace RecipeBook.Application.UserCases.User.Profile
{
    public interface IGetUserProfile
    {
        public Task<ResponseUserProfileJson> Execute();
    }
}
