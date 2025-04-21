using Moq;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories
{
    public class RecipeReadOnlyRepositoryBuilder
    {
        public readonly Mock<IRecipeReadOnlyRepository> _repository;

        public RecipeReadOnlyRepositoryBuilder() => _repository = new Mock<IRecipeReadOnlyRepository>();

        public IRecipeReadOnlyRepository Build() => _repository.Object;

        public RecipeReadOnlyRepositoryBuilder Filter(User user, IList<Recipe> recipes)
        {
            _repository.Setup(repository => repository.Filter(user, It.IsAny<FilterRecipesDto>())).ReturnsAsync(recipes);
            return this;
        }

        public RecipeReadOnlyRepositoryBuilder GetById(User user, Recipe? recipe)
        {
            if (recipe is not null)
                _repository.Setup(repository => repository.GetById(user, recipe.ID)).ReturnsAsync(recipe);
            return this;
        }

        public RecipeReadOnlyRepositoryBuilder IsValidRecipeOwner(User user, Recipe? recipe)
        {
            if (recipe is not null)
                _repository.Setup(repository => repository.IsValidRecipeOwner(user, recipe.ID)).ReturnsAsync(true);
            return this;
        }
    }
}
