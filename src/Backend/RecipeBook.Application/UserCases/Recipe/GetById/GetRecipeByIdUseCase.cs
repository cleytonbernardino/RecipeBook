using AutoMapper;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.GetById
{
    public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetRecipeByIdUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository repository,
            IMapper mapper
        )
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseRecipeJson> Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _repository.GetById(loggedUser, recipeId);

            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            return _mapper.Map<ResponseRecipeJson>(recipe);
        }
    }
}
