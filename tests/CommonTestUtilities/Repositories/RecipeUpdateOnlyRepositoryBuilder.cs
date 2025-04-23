using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories
{
    public class RecipeUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IRecipeUpdateOnlyRepository> _repository;

        public RecipeUpdateOnlyRepositoryBuilder() => _repository = new Mock<IRecipeUpdateOnlyRepository>();

        public IRecipeUpdateOnlyRepository Build() => _repository.Object;

        public RecipeUpdateOnlyRepositoryBuilder GetById(User user, Recipe? recipe)
        {
            if (recipe is not null)
                _repository.Setup(repository => repository.GetById(user, recipe.ID)).ReturnsAsync(recipe);
            return this;
        }
    }
}
