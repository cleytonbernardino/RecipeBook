using AutoMapper;
using RecipeBook.Application.Extensions;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;

namespace RecipeBook.Application.UserCases.Dashbord
{
    public class DashboardUseCase : IDashboardUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;

        public DashboardUseCase(
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

        public async Task<ResponsesRecipesJson> Execute()
        {
            var loggedUser = await _loggedUser.User();

            var recipes = await _repository.GetForDashbord(loggedUser);

            return new ResponsesRecipesJson()
            {
                Recipes = await recipes.MapToShortRecipeJson(loggedUser, _blobStorageService, _mapper)
            };
        }
    }
}
