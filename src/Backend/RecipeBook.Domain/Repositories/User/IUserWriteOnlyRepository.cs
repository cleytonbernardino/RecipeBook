namespace RecipeBook.Domain.Repositories.User
{
    public interface IUserWriteOnlyRepository
    {
        public Task Add(Entities.User user);

        public void Update(Entities.User user);
    }
}
