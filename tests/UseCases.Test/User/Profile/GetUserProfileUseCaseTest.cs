using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using RecipeBook.Application.UserCases.User.Profile;
using Shouldly;

namespace UseCases.Test.User.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build().user;

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute();

            result.ShouldNotBeNull();
            result.Name.ShouldBe(user.Name);
            result.Email.ShouldBe(user.Email);
        }

        private static GetUserProfileUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            var mapper = MapperBuilder.Build();
            var logged = LoggedUserBuilder.Build(user);
            return new GetUserProfileUseCase(logged, mapper);
        }
    }
}
