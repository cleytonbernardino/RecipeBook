using Bogus;
using RecipeBook.Communiction.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build(int passwordLength = 10)
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(user => user.Password, (f) => f.Internet.Password())
                .RuleFor(user => user.NewPassword, (f) => f.Internet.Password(passwordLength))
            ;
        }
    }
}
