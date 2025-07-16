using Microsoft.AspNetCore.Http;
using RecipeBook.Application.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.Image.GetImage;

public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public AddUpdateImageCoverUseCase(
        ILoggedUser loggedUser,
        IRecipeUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task Execute(long id, IFormFile file)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggedUser, id);

        if (recipe == null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        using (var fileStream = file.OpenReadStream())
        {
            (bool isValidImage, string extension) = fileStream.ValideAndGetImageExtension();

            if ( !isValidImage )
                throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

            if(string.IsNullOrEmpty(recipe.ImageIndentifier))
            {
                recipe.ImageIndentifier = $"{Guid.NewGuid()}{extension}";

                _repository.Update(recipe);

                await _unitOfWork.Commit();
            }

            await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIndentifier);
        }
    }
}
