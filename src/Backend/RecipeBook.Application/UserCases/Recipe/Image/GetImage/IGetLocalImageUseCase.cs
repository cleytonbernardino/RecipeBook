namespace RecipeBook.Application.UserCases.Recipe.Image.GetImage;

public interface IGetLocalImageUseCase
{
    Task<string> Execute(string userIndetifier, string imageName);
}
