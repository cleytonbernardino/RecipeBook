using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Recipe.Filter;
using RecipeBook.Communication.Enums;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Filter;

public class FilterRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();

        var user = UserBuilder.Build().user;

        var recipes = RecipeBuilder.Collection(user, 5);

        var useCase = CreateUseCase(user, recipes);
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Recipes.ShouldNotBeNull().ShouldNotBeEmpty();
        result.Recipes.Count.ShouldBeGreaterThan(1);
    }

    [Fact]
    public async Task Error_CookingTime_Invalid()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((CookingTime)1000);

        var user = UserBuilder.Build().user;

        var recipes = RecipeBuilder.Collection(user);

        var useCase = CreateUseCase(user, recipes);

        async Task act() { await useCase.Execute(request); };

        var result = await act().ShouldThrowAsync<ErrorOnValidationException>();
        result.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
    }

    private static FilterRecipeUseCase CreateUseCase(
        RecipeBook.Domain.Entities.User user, IList<RecipeBook.Domain.Entities.Recipe> recipes
    )
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder().Filter(user, recipes).Build();
        var mapper = MapperBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().GetImageUrl(user, recipes).Build();

        return new FilterRecipeUseCase(loggedUser, repository, mapper, blobStorage);
    }
}
