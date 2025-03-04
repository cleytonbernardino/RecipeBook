using AutoMapper;
using RecipeBook.Application.Cryptography;
using RecipeBook.Communiction.Requests;
using RecipeBook.Communiction.Responses;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly PasswordEncripter _passwordEncripter;

        public RegisterUserUseCase(
            IUserReadOnlyRepository readOnlyRepository,
            IUserWriteOnlyRepository writeOnlyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAccessTokenGenerator accessTokenGenerator,
            PasswordEncripter passwordEncripter
        )
        {
            _userReadOnlyRepository = readOnlyRepository;
            _userWriteOnlyRepository = writeOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accessTokenGenerator = accessTokenGenerator;
            _passwordEncripter = passwordEncripter;
        }

        public async Task<ResponseResgisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _passwordEncripter.Encript(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            await _userWriteOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            return new ResponseResgisteredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokenJson()
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
                }
            };
        }

        public async Task Validate(RequestRegisterUserJson request)
        {
            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            bool emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("", ResourceMessagesException.EMAIL_IN_USE));

            if (!result.IsValid)
            {
                var errMsg = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errMsg);
            }
        }
    }
}
