using Bogus;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateUserJsonBuilder
    {
        public static RequestUpdateUserJson Build()
        {
            return new Faker<RequestUpdateUserJson>()
                .RuleFor(user => user.Name, (f) => f.Person.FullName)
                .RuleFor(user => user.Email, (f) => f.Person.Email)
            ;
        }
    }
}
