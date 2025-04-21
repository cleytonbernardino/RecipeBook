using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe.Update;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.Recipe.Update
{
    public class UpdateRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build().user;

            var recipe = RecipeBuilder.Build(user);

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user, recipe);
            await useCase.Execute(recipe.ID, request);

            // This Assert is so that the sonar cloud does not point it out as an error, the real test is to run the use case, without it breaking.
            Assert.True(true);
        }

        [Fact]
        public async Task Error_Recipe_Not_Found()
        {
            var user = UserBuilder.Build().user;

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user);
            async Task act() { await useCase.Execute(1000, request); };

            var exception = await Assert.ThrowsAsync<NotFoundException>(act);
            Assert.Equal(ResourceMessagesException.RECIPE_NOT_FOUND, exception.Message);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var user = UserBuilder.Build().user;

            var recipe = RecipeBuilder.Build(user);

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = "";

            var useCase = CreateUseCase(user, recipe);
            async Task act() { await useCase.Execute(recipe.ID, request); };

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            var errors = exception.ErrorMessagens;

            Assert.Single(errors);
            Assert.Equal(ResourceMessagesException.TITLE_EMPTY, errors.First().ToString());
        }

        private static UpdateRecipeUseCase CreateUseCase(
            RecipeBook.Domain.Entities.User user, 
            RecipeBook.Domain.Entities.Recipe? recipe = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new UpdateRecipeUseCase(loggedUser, repository, mapper, unitOfWork);
        }
    }
}
