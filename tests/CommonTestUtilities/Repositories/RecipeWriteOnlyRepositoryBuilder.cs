using Moq;
using RecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories
{
    public class RecipeWriteOnlyRepositoryBuilder
    {
        public static IRecipeWriteOnlyRepository Build()
        {
            Mock<IRecipeWriteOnlyRepository> mock = new();
            return mock.Object;
        }
    }
}
