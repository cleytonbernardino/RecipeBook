namespace RecipeBook.Domain.Repositories.User
{
    public interface IUserUpdateOnlyRepository
    {
        public Task<Entities.User> GetById(long id);
        public void Update(Entities.User user);
    }
}
