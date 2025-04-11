using Moq;
using RecipeBook.Application.UserCases.Recipe.Filter;
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
    }
}
