using AutoMapper;
using RecipeBook.Application.Extensions;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.Register
{
    public class RecipeUseCase : IRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;
        private readonly IRecipeWriteOnlyRepository _repository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeUseCase(
            ILoggedUser loggedUser,
            IMapper mapper,
            IRecipeWriteOnlyRepository repository,
            IBlobStorageService blobStorageService,
            IUnitOfWork unitOfWork
        )
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
            _repository = repository;
            _blobStorageService = blobStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request)
        {
            Validator(request);

            var loggedUser = await _loggedUser.User();

            var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
            recipe.UserId = loggedUser.ID;

            var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
            for (int index = 0; index < instructions.Count; index++)
            {
                instructions[index].Step = index + 1;
            }
            recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

            if (request.Image is not null)
            {
                using (var fileStream = request.Image.OpenReadStream())
                {
                    (bool isValidImage, string extension) = fileStream.ValideAndGetImageExtension();

                    if ( !isValidImage)
                        throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

                    recipe.ImageIndentifier = $"{Guid.NewGuid()}{extension}";
                    await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIndentifier);
                }
            }

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
