using RecipeBook.Communiction.Requests;
using RecipeBook.Domain.Cryptography;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.User.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _userUpdateOnly;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository userUpdateOnly,
            IPasswordEncripter passwordEncripter,
            IUnitOfWork unitOfWork    
        )
        {
            _loggedUser = loggedUser;
            _userUpdateOnly = userUpdateOnly;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.User();

            Validator(request, loggedUser);
            
            var user = await _userUpdateOnly.GetById(loggedUser.ID);

            user.Password = _passwordEncripter.Encript(request.NewPassword);

            _userUpdateOnly.Update(user);
            await _unitOfWork.Commit();
        }

        private void Validator(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
        {
            ChangePasswordUseCaseValidator validator = new();
            var result = validator.Validate(request);

            string currentPasswordEncripted = _passwordEncripter.Encript(request.Password);

            if (!currentPasswordEncripted.Equals(loggedUser.Password))
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(
                    "", ResourceMessagesException.PASSWORD_DIFFERENT_FROM_CURRENT_PASSWORD
                ));

            if (!result.IsValid)
            {
                var errorMsg = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMsg);
            }
        }
    }
}
