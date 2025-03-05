namespace RecipeBook.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {
        public Task<bool> ExistActiveUserWithEmail(string email);

        public Task<Entities.User?> GetByEmailAndPassword(string email, string password);

        public Task<Entities.User> GetById(long id);

        public Task<bool> ExistActiveUserWithIndentifier(Guid indentifier);
    }
}
