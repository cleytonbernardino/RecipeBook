using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Attributes;
using RecipeBook.API.Binders;
using RecipeBook.Application.UserCases.Recipe.Delete;
using RecipeBook.Application.UserCases.Recipe.Filter;
using RecipeBook.Application.UserCases.Recipe.Generate;
using RecipeBook.Application.UserCases.Recipe.GetById;
using RecipeBook.Application.UserCases.Recipe.Image;
using RecipeBook.Application.UserCases.Recipe.Image.GetImage;
using RecipeBook.Application.UserCases.Recipe.Register;
using RecipeBook.Application.UserCases.Recipe.Update;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.API.Controllers;

[AuthenticatedUser]
public class RecipeController : RecipeBookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredRecipeJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRecipeJson request,
        [FromServices] IRecipeUseCase useCase)
    {
        var response = await useCase.Execute(request);
        return Created("", response);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(ResponseShortRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Filter(
        [FromBody] RequestFilterRecipeJson request, 
        [FromServices] IFilterRecipeUseCase useCase)
    {
        var response = await useCase.Execute(request);

        if (response.Recipes.Count != 0)
            return Ok(response);
        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecipe(
        [FromRoute] [ModelBinder(typeof(RecipeBookIdBinder))] long id,
        [FromServices] IGetRecipeByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] [ModelBinder(typeof(RecipeBookIdBinder))] long id,
        [FromServices] IUpdateRecipeUseCase useCase,
        [FromBody] RequestRecipeJson request)
    {
        await useCase.Execute(id, request);
        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] [ModelBinder(typeof(RecipeBookIdBinder))] long id,
        [FromServices] IDeleteRecipeUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [HttpPost("generate")]
    [ProducesResponseType(typeof(ResponseGeneratedRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Generate(
        [FromServices] IGenerateRecipeUseCase useCase,
        [FromBody] RequestGenerateRecipeJson request)
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }

    [HttpPut("image/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateImagem(
        [FromServices] IAddUpdateImageCoverUseCase useCase,
        [FromRoute][ModelBinder(typeof(RecipeBookIdBinder))] long id,
        IFormFile file)
    {
        await useCase.Execute(id, file);
        return NoContent();
    }

    [HttpGet("image/{userIndentifier}/{imageName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImage(
        [FromServices] IGetLocalImageUseCase useCase,
        string userIndentifier, string imageName
        )
    {
        string imageFullDir = await useCase.Execute(userIndentifier, imageName);
        string imageFormat = imageName.Split(".")[1];
        return PhysicalFile(imageFullDir, $"image/{imageFormat}", $"RecipeImage.{imageFormat}");
    }
}
