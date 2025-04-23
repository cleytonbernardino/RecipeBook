using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Login.DoLogin;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;

namespace Validators.Test.Login.DoLogin
{
    public class DoLoginValidatorTest
    {
        [Fact]
        public void Success()
        {
            RequestLoginJson request = RequestLoginJsonBuilder.Build();

            DoLoginUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_Email_Empty()
        {
            RequestLoginJson request = RequestLoginJsonBuilder.Build();
            request.Email = "";

            DoLoginUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.EMAIL_EMPTY, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            RequestLoginJson request = RequestLoginJsonBuilder.Build();
            request.Email = "email.com";

            DoLoginUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.EMAIL_INVALID, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Error_Password_Empty()
        {
            RequestLoginJson request = RequestLoginJsonBuilder.Build();
            request.Password = "";

            DoLoginUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(ResourceMessagesException.PASSWORD_EMPTY, result.Errors.First().ErrorMessage);
        }
    }
}
