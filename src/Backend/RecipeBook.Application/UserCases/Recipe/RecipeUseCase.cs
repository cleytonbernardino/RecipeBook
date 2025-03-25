using AutoMapper;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe
{
    public class RecipeUseCase : IRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;
        private readonly IRecipeWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeUseCase(
            ILoggedUser loggedUser,
            IMapper mapper,
            IRecipeWriteOnlyRepository repository,
            IUnitOfWork unitOfWork
        )
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request)
        {
            Validator(request);

            var loggedUser = await _loggedUser.User();

            var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
            recipe.UserId = loggedUser.ID;

            var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
            for (int index = 0; index < instructions.Count; index++)
            {
                instructions.ElementAt(index).Step = index + 1;
            }
            recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

            await _repository.Add(recipe);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
        }

        private static void Validator(RequestRecipeJson request)
        {
            RecipeValidator validator = new();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errMsg = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
                throw new ErrorOnValidationException(errMsg);
            }
        }
    }
}
