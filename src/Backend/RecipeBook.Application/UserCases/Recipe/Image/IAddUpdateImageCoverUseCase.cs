using Microsoft.AspNetCore.Http;

namespace RecipeBook.Application.UserCases.Recipe.Image;

public interface IAddUpdateImageCoverUseCase
{
    public Task Execute(long id, IFormFile file);
}
