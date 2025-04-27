using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe.Update;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

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
            async Task act() => await useCase.Execute(recipe.ID, request);

            await act().ShouldNotThrowAsync();
        }

        [Fact]
        public async Task Error_Recipe_Not_Found()
        {
            var user = UserBuilder.Build().user;

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user);
            async Task act() => await useCase.Execute(1000, request);

            var exception = await act().ShouldThrowAsync<NotFoundException>();
            exception.Message.ShouldBe(ResourceMessagesException.RECIPE_NOT_FOUND);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var user = UserBuilder.Build().user;

            var recipe = RecipeBuilder.Build(user);

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = "";

            var useCase = CreateUseCase(user, recipe);
            async Task act() => await useCase.Execute(recipe.ID, request);

            var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.TITLE_EMPTY);
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
