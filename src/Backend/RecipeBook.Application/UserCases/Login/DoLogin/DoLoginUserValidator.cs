using FluentValidation;
using RecipeBook.Communiction.Requests;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UserCases.Login.DoLogin
{
    public class DoLoginUserValidator : AbstractValidator<RequestLoginJson>
    {
        public DoLoginUserValidator()
        {
            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceMessagesException.PASSWORD_EMPTY);
            When(user => !string.IsNullOrEmpty(user.Email), () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });
        }
    }
}
