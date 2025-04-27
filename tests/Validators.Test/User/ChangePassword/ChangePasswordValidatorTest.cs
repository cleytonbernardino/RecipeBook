using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.ChangePassword;
using RecipeBook.Exceptions;
using Shouldly;
using Validators.Test.InlineData;

namespace Validators.Test.User.ChangePassword
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            ChangePasswordValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_NewPassword_Empty()
        {
            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = "";

            ChangePasswordValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);
        }

        [Theory]
        [ClassData(typeof(InlinePasswordMaxLenght))]
        public void Error_NewPassword_Invalid(int passwordLength)
        {
            var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

            ChangePasswordValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_LENGTH_INVALID);
        }
    }
}
