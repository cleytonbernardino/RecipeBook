using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe.Register;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

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

            result.Title.ShouldBe(request.Title);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var user = UserBuilder.Build().user;

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = "";

            var useCase = CreateUseCase(user);
            async Task act() => await useCase.Execute(request);

            var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.TITLE_EMPTY);
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
