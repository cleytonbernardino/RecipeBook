using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UserCases.Login.DoLogin;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.API.Controllers
{
    public class LoginController : RecipeBookBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResgisteredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] RequestLoginJson request,
            [FromServices] IDoLoginUseCase useCase
        )
        {
            ResponseResgisteredUserJson response = await useCase.Execute(request);
            return Ok(response);
        }
    }
}
