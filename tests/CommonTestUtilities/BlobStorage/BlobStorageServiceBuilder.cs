using Bogus;
using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Services.Storage;

namespace CommonTestUtilities.BlobStorage;

public class BlobStorageServiceBuilder
{
    private readonly Mock<IBlobStorageService> _mock;

    public BlobStorageServiceBuilder() => _mock = new Mock<IBlobStorageService>();

    public BlobStorageServiceBuilder GetImageUrl(User user, string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return this;

        Faker faker = new();
        var imageUrl = faker.Image.LoremFlickrUrl();

        _mock.Setup(blobStorage => blobStorage.GetImageUrl(user, fileName)).ReturnsAsync(imageUrl);

        return this;
    }

    public BlobStorageServiceBuilder GetImageUrl(User user, IList<Recipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            GetImageUrl(user, recipe.ImageIndentifier);
        }
        return this;
    }

    public IBlobStorageService Build() => _mock.Object;
}
