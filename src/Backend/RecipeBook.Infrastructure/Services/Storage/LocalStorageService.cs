using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Services.Storage;

namespace RecipeBook.Infrastructure.Services.Storage;

internal class LocalStorageService : IBlobStorageService
{
    private readonly string _image_dir_root;

    public LocalStorageService()
    {
        _image_dir_root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "images");
    }

    public async Task Upload(User user, Stream file, string fileName)
    {
        string imageDir = Path.Combine(_image_dir_root, user.UserIdentifier.ToString());

        if (file.CanSeek)
        {
            file.Seek(0, SeekOrigin.Begin);
        }

        if (!Directory.Exists(imageDir))
            Directory.CreateDirectory(imageDir);

        string fullPath = Path.Combine(imageDir, fileName);
        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            await file.CopyToAsync(fileStream);
        }
    }
}
