using Bogus;
using CommonTestUtilities.Cryptography;
using RecipeBook.Domain.Cryptography;
using RecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static (User user, string password) Build()
        {
            IPasswordEncripter passwordEncripter = PasswordEncripterBuilder.Build();
            string password = new Faker().Internet.Password();
            string encriptyPassword = passwordEncripter.Encript(password);

            var user = new Faker<User>()
                .RuleFor(user => user.ID, _ => 1)
                .RuleFor(user => user.Name, (f) => f.Person.FullName)
                .RuleFor(user => user.Email, (f) => f.Person.Email)
                .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid())
                .RuleFor(user => user.Password, (f) => encriptyPassword);

            return (user, password);
        }
    }
}
