using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Attributes;
using RecipeBook.Application.UserCases.Recipe;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.API.Controllers
{
    [AuthenticatedUser]
    public class RecipeController : RecipeBookBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredRecipeJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromBody] RequestRecipeJson request, [FromServices] IRecipeUseCase useCase
        )
        {
            var response = await useCase.Execute(request);
            return Created("", response);
        }
    }
}
