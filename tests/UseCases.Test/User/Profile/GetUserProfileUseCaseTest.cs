using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using RecipeBook.Application.UserCases.User.Profile;
using RecipeBook.Communication.Responses;

namespace UseCases.Test.User.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build().user;

            GetUserProfileUseCase useCase = CreateUseCase(user);

            ResponseUserProfileJson result = await useCase.Execute();

            Assert.NotNull(result);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Email, result.Email);
        }

        private static GetUserProfileUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            var mapper = MapperBuilder.Build();
            var logged = LoggedUserBuilder.Build(user);
            return new GetUserProfileUseCase(logged, mapper);
        }
    }
}
