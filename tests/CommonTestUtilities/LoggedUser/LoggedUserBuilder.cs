using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Services.LoggedUser;

namespace CommonTestUtilities.LoggedUser
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(User user)
        {
            Mock<ILoggedUser> mock = new();
            mock.Setup(x => x.User()).ReturnsAsync(user);
            return mock.Object;
        }
    }
}
