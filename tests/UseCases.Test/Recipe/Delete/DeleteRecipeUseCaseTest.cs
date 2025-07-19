using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using RecipeBook.Application.UserCases.Recipe.Delete;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Delete;

public class DeleteRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        async Task act() { await useCase.Execute(recipe.ID); };

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Recipe_Not_Found()
    {
        var user = UserBuilder.Build().user;

        var useCase = CreateUseCase(user);

        async Task act() { await useCase.Execute(1000); };

        var result = await act().ShouldThrowAsync(typeof(NotFoundException));
        result.Message.ShouldBe(ResourceMessagesException.RECIPE_NOT_FOUND);

    }

    private static DeleteRecipeUseCase CreateUseCase(RecipeBook.Domain.Entities.User user, RecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var readOnlyRepository = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var blobStorage = new BlobStorageServiceBuilder().GetImageUrl(user, recipe?.ImageIndentifier).Build();
        var writeOnlyRepository = RecipeWriteOnlyRepositoryBuilder.Build();
        var unityOfWork = UnitOfWorkBuilder.Build();

        return new DeleteRecipeUseCase(loggedUser, readOnlyRepository, blobStorage, writeOnlyRepository, unityOfWork);
    }
}
