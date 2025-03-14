using AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
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

            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();

            RecipeUseCase useCase = CreateUseCase(user);
            await useCase.Execute(request);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var user = UserBuilder.Build().user;

            RequestRecipeJson request = RequestRecipeJsonBuilder.Build();
            request.Title = "";

            RecipeUseCase useCase = CreateUseCase(user);
            async Task act() => await useCase.Execute(request);

            var exception = await Assert.ThrowsAnyAsync<ErrorOnValidationException>(act);
            Assert.Single(exception.ErrorMessagens);
            Assert.Equal(ResourceMessagesException.TITLE_EMPTY, exception.ErrorMessagens[0].ToString());
        }

        private static RecipeUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            ILoggedUser loggedUser = LoggedUserBuilder.Build(user);
            IMapper mapper = MapperBuilder.Build();
            IRecipeWriteOnlyRepository repository = RecipeWriteOnlyRepositoryBuilder.Build();
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();

            return new RecipeUseCase(loggedUser, mapper, repository, unitOfWork);
        }
    }
}
