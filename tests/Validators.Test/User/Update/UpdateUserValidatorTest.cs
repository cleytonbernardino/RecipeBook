using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.Update;
using RecipeBook.Communication.Requests;

namespace Validators.Test.User.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();

            UpdateUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_Name_Empty()
        {
            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Name = "";

            UpdateUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact]
        public void Error_Email_Empty()
        {
            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "";

            UpdateUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "email.com";

            UpdateUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }
    }
}
