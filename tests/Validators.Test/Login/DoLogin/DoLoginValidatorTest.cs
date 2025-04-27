using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.Login.DoLogin;
using RecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.Login.DoLogin
{
    public class DoLoginValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestLoginJsonBuilder.Build();

            DoLoginUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var request = RequestLoginJsonBuilder.Build();
            request.Email = "";

            DoLoginUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_EMPTY);
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var request = RequestLoginJsonBuilder.Build();
            request.Email = "email.com";

            DoLoginUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_INVALID);
        }

        [Fact]
        public void Error_Password_Empty()
        {
            var request = RequestLoginJsonBuilder.Build();
            request.Password = "";

            DoLoginUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);
        }
    }
}
