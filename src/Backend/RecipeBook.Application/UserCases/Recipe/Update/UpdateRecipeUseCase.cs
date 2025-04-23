using AutoMapper;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.Update
{
    public class UpdateRecipeUseCase : IUpdateRecipeUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeUpdateOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRecipeUseCase(
            ILoggedUser loggedUser,
            IRecipeUpdateOnlyRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long id, RequestRecipeJson request)
        {
            var loggedUser = await _loggedUser.User();

            Validate(request);

            var recipe = await _repository.GetById(loggedUser, id);

            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            recipe.Instructions.Clear();
            recipe.Ingredients.Clear();
            recipe.DishTypes.Clear();

            _mapper.Map(request, recipe);

            var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
            for (int index = 0; index < instructions.Count; index++)
            {
                instructions.ElementAt(index).Step = index + 1;
            }
            recipe.Instructions = _mapper.Map<IList<Instruction>>(instructions);

            _repository.Update(recipe);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestRecipeJson request)
        {
            RecipeValidator validator = new();
            var result = validator.Validate(request);

            if( !result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
 