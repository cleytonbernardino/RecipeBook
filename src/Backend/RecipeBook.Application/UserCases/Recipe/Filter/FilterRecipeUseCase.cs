using AutoMapper;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.Filter
{
    public class FilterRecipeUseCase : IFilterRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _respository;
        private readonly IMapper _mapper;

        public FilterRecipeUseCase(
            ILoggedUser loggedUser, IRecipeReadOnlyRepository repository, IMapper mapper 
        )
        {
            _loggedUser = loggedUser;
            _respository = repository;
            _mapper = mapper;
        }

        public async Task<ResponsesRecipesJson> Execute(RequestFilterRecipeJson request)
        {
            Validate(request);
            
            var user = await _loggedUser.User();

            FilterRecipesDto filters = new()
            {
                RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
                CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.CookingTime)c).ToList(),
                Difficulties = request.Difficulties.Distinct().Select(c => (Domain.Enums.Difficulty)c).ToList(),
                DishTypes = request.DishTypes.Distinct().Select(c => (Domain.Enums.DishType)c).ToList()
            };

            var recipes  = await _respository.Filter(user, filters);

            return new ResponsesRecipesJson()
            {
                Recipes = _mapper.Map<List<ResponseShortRecipeJson>>(recipes)
            };
        }

        private static void Validate(RequestFilterRecipeJson request)
        {
            FilterRecipeValidator validator = new();
            var result = validator.Validate(request);

            if (result.IsValid)
            {
                var errMsg = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errMsg);
            }
        }
    }
}
