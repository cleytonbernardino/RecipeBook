using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Attributes;
using RecipeBook.Application.UserCases.User.Profile;
using RecipeBook.Application.UserCases.User.Register;
using RecipeBook.Application.UserCases.User.Update;
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
            [FromServices] IRegisterUserUseCase    useCase
        )
        {
            ResponseResgisteredUserJson result = await useCase.Execute(request);
            return Created("", result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
        [AuthenticatedUser]
        public async Task<IActionResult> GetProfile([FromServices] IGetUserProfile useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]
        public async Task<IActionResult> UpdateProfile(
            [FromBody]     RequestUpdateUserJson request,
            [FromServices] IUpdateUserProfile    useCase
        )
        {
            await useCase.Execute(request);
            return NoContent();
        }
    }
}
