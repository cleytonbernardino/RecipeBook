using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Services.Storage;

public interface IBlobStorageService
{
    Task Upload(User user, Stream file, string fileName);
}
