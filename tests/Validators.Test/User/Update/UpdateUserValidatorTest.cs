using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.Update;
using RecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            UpdateUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = "";

            UpdateUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "";

            UpdateUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_EMPTY);
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "email.com";

            UpdateUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_INVALID);
        }
    }
}
