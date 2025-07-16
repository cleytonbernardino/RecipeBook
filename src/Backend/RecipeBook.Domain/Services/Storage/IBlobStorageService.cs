using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Services.Storage;

public interface IBlobStorageService
{
    Task Upload(User user, Stream file, string fileName);
    Task<string> GetImageUrl(User user, string fileName);
    Task Delete(User user, string fileName);
}
