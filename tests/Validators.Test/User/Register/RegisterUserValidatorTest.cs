using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.Register;
using RecipeBook.Exceptions;
using Shouldly;
using Validators.Test.InlineData;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Succes()
        {
            var request = RequestUserJsonBuilder.Build();

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Erro_Name_Empty()
        {
            var request = RequestUserJsonBuilder.Build();
            request.Name = "";

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Erro_Email_Empty()
        {
            var request = RequestUserJsonBuilder.Build();
            request.Email = "";

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_EMPTY);
        }

        [Fact]
        public void Erro_Email_Invalid()
        {
            var request = RequestUserJsonBuilder.Build();
            request.Email = "email.com";

            RegisterUserValidator validator = new();
            var result = validator.Validate(request); ;

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_INVALID);
        }

        [Theory]
        [ClassData(typeof(InlinePasswordMaxLenght))]
        public void Erro_Password_Lenght_Invalid(int passwordLenght)
        {
            var request = RequestUserJsonBuilder.Build(passwordLenght);

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_LENGTH_INVALID);
        }

        [Fact]
        public void Erro_Password_Empty()
        {
            var request = RequestUserJsonBuilder.Build();
            request.Password = "";

            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);
        }
    }
}
