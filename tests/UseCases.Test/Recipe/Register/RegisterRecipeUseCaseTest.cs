using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using RecipeBook.Application.UserCases.Recipe.Register;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Register;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Success_without_image()
    {
        var user = UserBuilder.Build().user;

        var request = RequestRegisterRecipeFormDataBuilder.Build();

        var useCase = CreateUseCase(user);
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBeNullOrWhiteSpace();
        result.Title.ShouldBe(request.Title);
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success_with_image(IFormFile file)
    {
        var user = UserBuilder.Build().user;

        var request = RequestRegisterRecipeFormDataBuilder.Build(file);

        var useCase = CreateUseCase(user);
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBeNullOrWhiteSpace();
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var user = UserBuilder.Build().user;

        var request = RequestRegisterRecipeFormDataBuilder.Build();
        request.Title = "";

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute(request);

        var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
        exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.TITLE_EMPTY);
    }

    [Fact]
    public async Task Error_Invalid_File()
    {
        var user = UserBuilder.Build().user;

        var textFile = FormFileBuilder.Txt();

        var request = RequestRegisterRecipeFormDataBuilder.Build(textFile);

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute(request);

        var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
        exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.ONLY_IMAGES_ACCEPTED);
    }

    private static RecipeUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var mapper = MapperBuilder.Build();
        var repository = RecipeWriteOnlyRepositoryBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new RecipeUseCase(loggedUser, mapper, repository, blobStorage, unitOfWork);
    }
}
