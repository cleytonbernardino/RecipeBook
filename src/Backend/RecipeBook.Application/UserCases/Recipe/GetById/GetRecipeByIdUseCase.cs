using AutoMapper;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IBlobStorageService _blobStorageService;

    public GetRecipeByIdUseCase(
        ILoggedUser loggedUser,
        IRecipeReadOnlyRepository repository,
        IMapper mapper,
        IBlobStorageService blobStorageService
    )
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _mapper = mapper;
        _blobStorageService = blobStorageService;
    }

    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        var response = _mapper.Map<ResponseRecipeJson>(recipe);

        if ( !string.IsNullOrEmpty(recipe.ImageIndentifier))
        {
            var url = await _blobStorageService.GetImageUrl(loggedUser, recipe.ImageIndentifier);
            response.ImageUrl = url;
        }
        return response;
    }
}
