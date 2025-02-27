using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.Register;
using RecipeBook.Communiction.Requests;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest : RequestUserJsonBuilder
    {
        [Fact]
        public void Succes()
        {
            RegisterUserValidator validator = new();

            RequestRegisterUserJson request = MakeRequest();

            bool result = validator.Validate(request).IsValid;

            Assert.True(result);
        }
    }
}
