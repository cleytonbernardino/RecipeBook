using Microsoft.AspNetCore.Http;

namespace RecipeBook.Communication.Requests;

public class RequestRegisterRecipeFormData : RequestRecipeJson
{
    public IFormFile? Image { get; set; }
}
