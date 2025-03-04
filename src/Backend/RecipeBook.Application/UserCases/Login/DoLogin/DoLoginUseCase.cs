using RecipeBook.Application.Cryptography;
using RecipeBook.Communiction.Requests;
using RecipeBook.Communiction.Responses;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnly;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly PasswordEncripter _passwordEncripter;

        public DoLoginUseCase(
            IUserReadOnlyRepository userReadOnly,
            IAccessTokenGenerator accessTokenGenerator,
            PasswordEncripter encripter
        )
        {
            _userReadOnly = userReadOnly;
            _accessTokenGenerator = accessTokenGenerator;
            _passwordEncripter = encripter;
        }

        public async Task<ResponseResgisteredUserJson> Execute(RequestLoginJson request)
        {
            Validator(request);

            string passwordEncripty = _passwordEncripter.Encript(request.Password);
            Domain.Entities.User? user = await _userReadOnly.GetByEmailAndPassword(request.Email, passwordEncripty)
                ?? throw new InvalidLoginException();

            return new ResponseResgisteredUserJson()
            {
                Name = user.Name,
                Tokens = new ResponseTokenJson()
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
                }
            };
        }

        private static void Validator(RequestLoginJson request)
        {
            DoLoginUserValidator validator = new();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errMsg = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errMsg);
            }
        }
    }
}
