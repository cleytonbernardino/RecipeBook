using RecipeBook.Domain.Repositories;

namespace RecipeBook.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecipeBookDbContext _dbContext;

        public UnitOfWork(RecipeBookDbContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
