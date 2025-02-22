using RecipeBook.Communiction.Requests;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Communiction.Responses;

namespace RecipeBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResgisteredUserJson), StatusCodes.Status201Created)]
        public IActionResult Register(RequestRegisterUserJson request)
        {
            return Created();
        }
    }
}
