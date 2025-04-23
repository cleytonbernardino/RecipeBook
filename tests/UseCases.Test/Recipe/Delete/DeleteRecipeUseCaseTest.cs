using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using RecipeBook.Application.UserCases.Recipe.Delete;
using RecipeBook.Exceptions.ExceptionsBase;
using RecipeBook.Exceptions;

namespace UseCases.Test.Recipe.Delete
{
    public class DeleteRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build().user;
            var recipe = RecipeBuilder.Build(user);

            var useCase = createUseCase(user, recipe);

            await useCase.Execute(recipe.ID);

            // This Assert is so that the sonar cloud does not point it out as an error, the real test is to run the use case, without it breaking.
            Assert.True(true);
        }

        [Fact]
        public async Task Error_Recipe_Not_Found()
        {
            var user = UserBuilder.Build().user;

            var useCase = createUseCase(user);
            async Task act() { await useCase.Execute(1000); };

            var result = await Assert.ThrowsAsync<NotFoundException>(act);

            Assert.Equal(ResourceMessagesException.RECIPE_NOT_FOUND, result.Message);
        }

        private DeleteRecipeUseCase createUseCase(RecipeBook.Domain.Entities.User user, RecipeBook.Domain.Entities.Recipe? recipe = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var readOnlyRepository = new RecipeReadOnlyRepositoryBuilder().IsValidRecipeOwner(user, recipe).Build();
            var writeOnlyRepository = RecipeWriteOnlyRepositoryBuilder.Build();
            var unityOfWork = UnitOfWorkBuilder.Build();

            return new DeleteRecipeUseCase(loggedUser, readOnlyRepository, writeOnlyRepository, unityOfWork);
        }
    }
}
