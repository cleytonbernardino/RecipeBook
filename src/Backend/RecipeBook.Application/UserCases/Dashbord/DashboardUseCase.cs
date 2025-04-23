using AutoMapper;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;

namespace RecipeBook.Application.UserCases.Dashbord
{
    public class DashboardUseCase : IDashboardUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public DashboardUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository repository,
            IMapper mapper
        )
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponsesRecipesJson> Execute()
        {
            var user = await _loggedUser.User();

            var recipes = await _repository.GetForDashbord(user);

            return new ResponsesRecipesJson() {
                Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
            };
        }
    }
}
