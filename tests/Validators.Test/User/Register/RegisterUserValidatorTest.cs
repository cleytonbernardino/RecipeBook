using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.Register;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using Validators.Test.InlineData;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Succes()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();

            RegisterUserValidator validator = new();
            bool result = validator.Validate(request).IsValid;

            Assert.True(result);
        }

        [Fact]
        public void Erro_Name_Empty()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            request.Name = "";

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors[0].ErrorMessage, ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Erro_Email_Empty()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            request.Email = "";

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors[0].ErrorMessage, ResourceMessagesException.EMAIL_EMPTY);
        }

        [Fact]
        public void Erro_Email_Invalid()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            request.Email = "email.com";

            RegisterUserValidator validator = new();
            var result = validator.Validate(request); ;

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors[0].ErrorMessage, ResourceMessagesException.EMAIL_INVALID);
        }

        [Theory]
        [ClassData(typeof(InlinePasswordMaxLenght))]
        public void Erro_Password_Lenght_Invalid(int passwordLenght)
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build(passwordLenght);

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors[0].ErrorMessage, ResourceMessagesException.PASSWORD_LENGTH_INVALID);
        }

        [Fact]
        public void Erro_Password_Empty()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            request.Password = "";

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors[0].ErrorMessage, ResourceMessagesException.PASSWORD_EMPTY);
        }
    }
}
