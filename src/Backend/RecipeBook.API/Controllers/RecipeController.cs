using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Attributes;
using RecipeBook.API.Binders;
using RecipeBook.Application.UserCases.Recipe;
using RecipeBook.Application.UserCases.Recipe.Filter;
using RecipeBook.Application.UserCases.Recipe.GetById;
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
        public async Task<IActionResult> Filter(
            [FromBody] RequestFilterRecipeJson request, [FromServices] IFilterRecipeUseCase useCase
        )
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
            [FromServices] IGetRecipeByIdUseCase useCase
        )
        {
            var response = await useCase.Execute(id);
            return Ok(response);
        }
    }
}
