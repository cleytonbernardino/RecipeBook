using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.Delete
{
    public class DeleteRecipeUseCase : IDeleteRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _readOnlyRepository;
        private readonly IRecipeWriteOnlyRepository _writeOnlyRepository;
        private readonly IUnitOfWork _unitOFWork;

        public DeleteRecipeUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository readOnlyRespository,
            IRecipeWriteOnlyRepository writeOnlyRepository,
            IUnitOfWork unitOfWork
        )
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRespository;
            _writeOnlyRepository = writeOnlyRepository;
            _unitOFWork = unitOfWork;
        }

        public async Task Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _readOnlyRepository.IsValidRecipeOwner(loggedUser, recipeId);

            if ( !recipe )
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            await _writeOnlyRepository.Delete(recipeId);
            
            await _unitOFWork.Commit();
        }
    }
}
