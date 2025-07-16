using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Domain.ValueObjects;

namespace RecipeBook.Infrastructure.Services.Storage;

internal class AzureStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task Upload(User user, Stream file, string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
        await container.CreateIfNotExistsAsync();

        var blobClient = container.GetBlobClient(fileName);

        await blobClient.UploadAsync(file, overwrite: true);
    }

    public async Task<string> GetImageUrl(User user, string fileName)
    {
        var containerName = user.UserIdentifier.ToString();
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        
        var exist = await container.ExistsAsync();
        if (!exist)
            return string.Empty;

        var blobClient = container.GetBlobClient(fileName);
        exist = await blobClient.ExistsAsync();
        if (exist)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = fileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(RecipeBookRuleConstants.MAXIMUN_IMAGE_URL_LIFETIME_IN_MINUTES)
            };

            sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);
            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }
        return string.Empty;
    }

    public async Task Delete(User user, string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
        var exist = await container.ExistsAsync();
        if (exist)
        {
            await container.DeleteBlobIfExistsAsync(fileName);
        }
    }
}
