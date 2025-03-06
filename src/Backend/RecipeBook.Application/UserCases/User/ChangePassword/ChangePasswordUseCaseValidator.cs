using FluentValidation;
using RecipeBook.Application.SharedValidators;
using RecipeBook.Communiction.Requests;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UserCases.User.ChangePassword
{
    class ChangePasswordUseCaseValidator : AbstractValidator<RequestChangePasswordJson>
    {
        public ChangePasswordUseCaseValidator()
        {
            RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceMessagesException.PASSWORD_EMPTY);
            RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
        }
    }
}
