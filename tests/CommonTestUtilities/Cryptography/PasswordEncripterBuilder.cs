using RecipeBook.Application.Cryptography;
using RecipeBook.Domain.Cryptography;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        public static IPasswordEncripter Build() => new Sha512Encripter("test");
    }
}
