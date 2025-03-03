using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UserCases.User.Register;
using RecipeBook.Communiction.Requests;
using RecipeBook.Communiction.Responses;

namespace RecipeBook.API.Controllers
{
    public class UserController : RecipeBookBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResgisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromBody]     RequestRegisterUserJson request,
            [FromServices] IRegisterUserUseCase useCase
        )
        {
            ResponseResgisteredUserJson result = await useCase.Execute(request);
            return Created("", result);
        }
    }
}
