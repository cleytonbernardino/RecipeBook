using AutoMapper;
using RecipeBook.Application.Extensions;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.Filter
{
    public class FilterRecipeUseCase : IFilterRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _respository;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;

        public FilterRecipeUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository repository,
            IMapper mapper,
            IBlobStorageService blobStorageService
        )
        {
            _loggedUser = loggedUser;
            _respository = repository;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
        }

        public async Task<ResponsesRecipesJson> Execute(RequestFilterRecipeJson request)
        {
            Validate(request);
            
            var loggedUser = await _loggedUser.User();

            FilterRecipesDto filters = new()
            {
                RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
                CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.CookingTime)c).ToList(),
                Difficulties = request.Difficulties.Distinct().Select(c => (Domain.Enums.Difficulty)c).ToList(),
                DishTypes = request.DishTypes.Distinct().Select(c => (Domain.Enums.DishType)c).ToList()
            };

            var recipes  = await _respository.Filter(loggedUser, filters);

            return new ResponsesRecipesJson()
            {
                Recipes = await recipes.MapToShortRecipeJson(loggedUser, _blobStorageService, _mapper)
            };
        }

        private static void Validate(RequestFilterRecipeJson request)
        {
            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errMsg = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
                throw new ErrorOnValidationException(errMsg);
            }
        }
    }
}
