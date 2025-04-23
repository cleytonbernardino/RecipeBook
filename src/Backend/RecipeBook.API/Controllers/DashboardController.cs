using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Attributes;
using RecipeBook.Application.UserCases.Dashbord;
using RecipeBook.Communication.Responses;

namespace RecipeBook.API.Controllers
{
    [AuthenticatedUser]
    public class DashboardController : RecipeBookBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsesRecipesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromServices] IDashboardUseCase useCase)
        {
            var response = await useCase.Execute();

            if (response.Recipes.Any())
                return Ok(response);
            return NoContent();
        }
    }
}
