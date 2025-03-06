using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.ChangePassword;
using RecipeBook.Communiction.Requests;
using RecipeBook.Exceptions;
using Validators.Test.InlineData;

namespace Validators.Test.User.ChangePassword
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();

            ChangePasswordValidator validator = new();
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_NewPassword_Empty()
        {
            RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = "";

            ChangePasswordValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.PASSWORD_EMPTY, result.Errors[0].ToString());
        }

        [Theory]
        [ClassData(typeof(InlinePasswordMaxLenght))]
        public void Error_NewPassword_Invalid(int passwordLength)
        {
            RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build(passwordLength);

            ChangePasswordValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.PASSWORD_LENGTH_INVALID, result.Errors[0].ToString());
        }
    }
}
