using Bogus;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestLoginJsonBuilder
    {
        public static RequestLoginJson Build()
        {
            string provider = "gmail.com";
            return new Faker<RequestLoginJson>()
                .RuleFor(user => user.Email, (f, user) => f.Internet.Email(null, null, provider))
                .RuleFor(user => user.Password, (f) => f.Internet.Password());
        }
    }
}
