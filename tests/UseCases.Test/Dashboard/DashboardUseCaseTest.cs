using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using RecipeBook.Application.UserCases.Dashbord;
using Shouldly;

namespace UseCases.Test.Dashboard;

public class DashboardUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;

        var recipes = RecipeBuilder.Collection(user);

        var useCase = CreateUseCase(user, recipes);

        var result = await useCase.Execute();
        
        result.ShouldNotBeNull();
        result.Recipes.Count.ShouldBeGreaterThan(0);
        result.Recipes.ShouldSatisfyAllConditions(recipe => {
            recipe.First().Id.ShouldNotBeNullOrWhiteSpace();
            recipe.First().Title.ShouldNotBeNullOrWhiteSpace();
            recipe.First().AmountIngredients.ShouldBeGreaterThan(0);
        });
    }

    private static DashboardUseCase CreateUseCase(
        RecipeBook.Domain.Entities.User user,
        IList<RecipeBook.Domain.Entities.Recipe> recipes)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder().GetForDashboard(user, recipes).Build();
        var mapper = MapperBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().GetImageUrl(user, recipes).Build();

        return new DashboardUseCase(loggedUser, repository, mapper, blobStorage);
    }
}
