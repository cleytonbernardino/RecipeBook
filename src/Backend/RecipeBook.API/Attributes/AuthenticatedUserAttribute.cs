using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Filter;

namespace RecipeBook.API.Attributes
{
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
        {
        }
    }
}
