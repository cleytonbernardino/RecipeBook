using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.Register;
using RecipeBook.Communiction.Requests;
using RecipeBook.Exceptions;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest : RequestUserJsonBuilder
    {
        [Fact]
        public void Succes()
        {
            RequestRegisterUserJson request = MakeRequest();

            RegisterUserValidator validator = new();
            bool result = validator.Validate(request).IsValid;

            Assert.True(result);
        }

        [Fact]
        public void Erro_Name_Empty()
        {
            RequestRegisterUserJson request = MakeRequest();
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
            RequestRegisterUserJson request = MakeRequest();
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
            RequestRegisterUserJson request = MakeRequest();
            request.Email = "email.com";

            RegisterUserValidator validator = new();
            var result = validator.Validate(request); ;

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors[0].ErrorMessage, ResourceMessagesException.EMAIL_INVALID);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Erro_Password_Lenght_Invalid(int passwordLenght)
        {
            RequestRegisterUserJson request = MakeRequest(passwordLenght);

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors[0].ErrorMessage, ResourceMessagesException.PASSWORD_LENGTH_INVALID);
        }
    }
}
