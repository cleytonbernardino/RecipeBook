using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using RecipeBook.Application.UserCases.Recipe.Image.GetImage;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Image;

public class AddUpdateImageCoverUseCaseTest
{
    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success(IFormFile formFile)
    {
        var user = UserBuilder.Build().user;

        var recipe = RecipeBuilder.Build(user);

        var useCase = createUseCase(user, recipe);
        async Task act() => await useCase.Execute(recipe.ID, formFile);

        await act().ShouldNotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Error_Recipe_NotFound(IFormFile formFile)
    {
        var user = UserBuilder.Build().user;

        var useCase = createUseCase(user);
        async Task act() => await useCase.Execute(10, formFile);

        var exception = await act().ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    [Fact]
    public async Task Error_File_Is_Txt()
    {
        var user = UserBuilder.Build().user;

        var recipe = RecipeBuilder.Build(user);

        var formFile = FormFileBuilder.Txt();

        var useCase = createUseCase(user, recipe);
        async Task act() => await useCase.Execute(recipe.ID, formFile);

        var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
        exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.ONLY_IMAGES_ACCEPTED);
    }

    private static AddUpdateImageCoverUseCase createUseCase(
        RecipeBook.Domain.Entities.User user,
        RecipeBook.Domain.Entities.Recipe? recipe=null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var unityOfWork = UnitOfWorkBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().Build();

        return new AddUpdateImageCoverUseCase(loggedUser, repository, unityOfWork, blobStorage);
    }
}
