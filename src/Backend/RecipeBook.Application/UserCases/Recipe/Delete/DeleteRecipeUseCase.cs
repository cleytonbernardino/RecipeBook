using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.Delete
{
    public class DeleteRecipeUseCase : IDeleteRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _readOnlyRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IRecipeWriteOnlyRepository _writeOnlyRepository;
        private readonly IUnitOfWork _unitOFWork;

        public DeleteRecipeUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository readOnlyRespository,
            IBlobStorageService blobStorageService,
            IRecipeWriteOnlyRepository writeOnlyRepository,
            IUnitOfWork unitOfWork
        )
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRespository;
            _blobStorageService = blobStorageService;
            _writeOnlyRepository = writeOnlyRepository;
            _unitOFWork = unitOfWork;
        }

        public async Task Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _readOnlyRepository.GetById(loggedUser, recipeId);

            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            if ( !string.IsNullOrEmpty(recipe.ImageIndentifier) )
                await _blobStorageService.Delete(loggedUser, recipe.ImageIndentifier);

            await _writeOnlyRepository.Delete(recipeId);
            
            await _unitOFWork.Commit();
        }
    }
}
