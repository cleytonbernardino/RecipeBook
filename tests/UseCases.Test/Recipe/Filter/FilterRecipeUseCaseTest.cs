using AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe.Filter;
using RecipeBook.Communication.Enums;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.Recipe.Filter
{
    public class FilterRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            RequestFilterRecipeJson request = RequestFilterRecipeJsonBuilder.Build();

            var user = UserBuilder.Build().user;

            var recipes = RecipeBuilder.Collection(user, 5);

            FilterRecipeUseCase useCase = CreateUseCase(user, recipes);
            var result = await useCase.Execute(request);

            Assert.NotNull(result);
            Assert.NotNull(result.Recipes);
            Assert.NotEmpty(result.Recipes);
            Assert.True(recipes.Count > 1);
        }

        [Fact]
        public async Task Error_CookingTime_Invalid()
        {
            RequestFilterRecipeJson request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((CookingTime)1000);

            var user = UserBuilder.Build().user;

            var recipes = RecipeBuilder.Collection(user);

            FilterRecipeUseCase useCase = CreateUseCase(user, recipes);

            Func<Task> act = async () => await useCase.Execute(request);

            var result = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            var errors = result.ErrorMessagens;

            Assert.Single(errors);
            Assert.Equal(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED, errors.First().ToString());
        }

        private static FilterRecipeUseCase CreateUseCase(
            RecipeBook.Domain.Entities.User user, IList<RecipeBook.Domain.Entities.Recipe> recipes
        )
        {
            ILoggedUser loggedUser = LoggedUserBuilder.Build(user);
            IRecipeReadOnlyRepository repository = new RecipeReadOnlyRepositoryBuilder().Filter(user, recipes).Build();
            IMapper mapper = MapperBuilder.Build();

            return new FilterRecipeUseCase(loggedUser, repository, mapper);
        }
    }
}
