using RecipeBook.Application.Cryptography;
using RecipeBook.Communiction.Requests;
using RecipeBook.Communiction.Responses;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnly;
        private readonly PasswordEncripter _passwordEncripter;

        public DoLoginUseCase(IUserReadOnlyRepository userReadOnly, PasswordEncripter encripter)
        {
            _userReadOnly = userReadOnly;
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
                Name = user.Name
            };
        }

        public void Validator(RequestLoginJson request)
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
