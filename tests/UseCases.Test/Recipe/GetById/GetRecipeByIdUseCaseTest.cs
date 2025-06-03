using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using RecipeBook.Application.UserCases.Recipe.GetById;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.GetById
{
    public class GetRecipeByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build().user;
            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, recipe);
            var result = await useCase.Execute(recipe.ID);

            result.ShouldNotBeNull();
            result.Id.ShouldNotBeNullOrEmpty();
            result.Title.ShouldBe(recipe.Title);
        }

        [Fact]
        public async Task Error_Recipe_Not_Found()
        {
            var user = UserBuilder.Build().user;
            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, recipe);
            async Task act() { await useCase.Execute(recipe.ID + 1); }

            var exeption = await act().ShouldThrowAsync<NotFoundException>();
            exeption.Message.ShouldBe(ResourceMessagesException.RECIPE_NOT_FOUND);
        }

        private static GetRecipeByIdUseCase CreateUseCase(
            RecipeBook.Domain.Entities.User user,
            RecipeBook.Domain.Entities.Recipe? recipe = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);

            var repository = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();

            var mapper = MapperBuilder.Build();

            return new GetRecipeByIdUseCase(loggedUser, repository, mapper);
        }
    }
}
