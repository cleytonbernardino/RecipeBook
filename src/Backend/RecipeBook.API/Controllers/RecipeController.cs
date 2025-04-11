using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Attributes;
using RecipeBook.Application.UserCases.Recipe;
using RecipeBook.Application.UserCases.Recipe.Filter;
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

        [HttpPost("filter")]
        [ProducesResponseType(typeof(ResponseShortRecipeJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Filter (
            [FromBody] RequestFilterRecipeJson request, [FromServices] IFilterRecipeUseCase useCase
        )
        {
            var response = await useCase.Execute(request);

            if (response.Recipes.Count != 0)
                return Ok(response);
            return NoContent();
        }
    }
}
