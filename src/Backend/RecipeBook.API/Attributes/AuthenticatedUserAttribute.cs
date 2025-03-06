using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Filter;

namespace RecipeBook.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
        {
        }
    }
}
