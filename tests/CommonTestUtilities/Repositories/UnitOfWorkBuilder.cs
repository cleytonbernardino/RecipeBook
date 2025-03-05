using Moq;
using RecipeBook.Domain.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build()
        {
            Mock<IUnitOfWork> mock = new();
            return mock.Object;
        }
    }
}
