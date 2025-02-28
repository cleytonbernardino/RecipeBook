using Bogus;
using RecipeBook.Communiction.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestUserJsonBuilder
    {
        public static RequestRegisterUserJson MakeRequest(int passwordLenght = 10)
        {
            string provider = "gmail.com";
            return new Faker<RequestRegisterUserJson>()
                .RuleFor(user => user.Name, (f) => f.Person.FullName)
                .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name, null, provider))
                .RuleFor(user => user.Password, (f) => f.Internet.Password(passwordLenght));
        }
    }
}
