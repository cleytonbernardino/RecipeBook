using FluentValidation;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UserCases.Recipe
{
    public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipeJson>
    {
        public FilterRecipeValidator()
        {
            RuleFor(recipe => recipe.RecipeTitle_Ingredient).NotEmpty().WithMessage(ResourceMessagesException.TITLE_EMPTY);
            RuleFor(recipe => recipe.CookingTimes).IsInEnum().WithMessage(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
            RuleFor(recipe => recipe.Difficulties).IsInEnum().WithMessage(ResourceMessagesException.DIFFICULTY_NOT_SUPPORTED);
            RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
        }
    }
}
