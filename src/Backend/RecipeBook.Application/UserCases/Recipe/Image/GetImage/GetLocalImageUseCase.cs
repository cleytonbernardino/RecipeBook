using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.Image.GetImage;

public class GetLocalImageUseCase : IGetLocalImageUseCase
{
    private readonly ILoggedUser _loggedUser;

    public GetLocalImageUseCase(
        ILoggedUser loggedUser
    )
    {
        _loggedUser = loggedUser;
    }

    public async Task<string> Execute(string userIndetifier, string imageName)
    {
        var user = await _loggedUser.User();
        if (user.UserIdentifier.ToString() != userIndetifier)
            throw new NotFoundException(ResourceMessagesException.IMAGE_CANNOT_BE_FOUND);

        string imageDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "images");
        string imageFile = Path.Combine(imageDir, userIndetifier, imageName);
        if (!File.Exists(imageFile))
            throw new NotFoundException(ResourceMessagesException.IMAGE_CANNOT_BE_FOUND);
        return imageFile;
    }
}
