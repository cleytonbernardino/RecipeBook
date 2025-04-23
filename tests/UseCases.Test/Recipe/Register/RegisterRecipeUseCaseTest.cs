using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.Recipe.Register
{
    public class RegisterRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build().user;

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user);
            var result = await useCase.Execute(request);

            Assert.Equal(request.Title, result.Title);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var user = UserBuilder.Build().user;

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = "";

            var useCase = CreateUseCase(user);
            async Task act() => await useCase.Execute(request);

            var exception = await Assert.ThrowsAnyAsync<ErrorOnValidationException>(act);
            Assert.Single(exception.ErrorMessagens);
            Assert.Equal(ResourceMessagesException.TITLE_EMPTY, exception.ErrorMessagens[0].ToString());
        }

        private static RecipeUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var repository = RecipeWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new RecipeUseCase(loggedUser, mapper, repository, unitOfWork);
        }
    }
}
