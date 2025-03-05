using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> User();
    }
}
