using AutoMapper;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Services.Storage;

namespace RecipeBook.Application.Extensions;

internal static class RecipeListExtension
{
    public static async Task<IList<ResponseShortRecipeJson>> MapToShortRecipeJson(
        this IList<Recipe> recipes, User user, IBlobStorageService blobStorageService, IMapper mapper)
    {
        var result = recipes.Select(async recipe =>
        {
            var response = mapper.Map<ResponseShortRecipeJson>(recipe);

            if (!string.IsNullOrEmpty(recipe.ImageIndentifier))
            {
                response.ImageUrl = await blobStorageService.GetImageUrl(user, recipe.ImageIndentifier);
            }
            return response;
        });

        return await Task.WhenAll(result);
    }
}
