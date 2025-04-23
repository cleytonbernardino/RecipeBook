using FluentValidation;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UserCases.Recipe.Register
{
    public class RecipeValidator : AbstractValidator<RequestRecipeJson>
    {
        public RecipeValidator()
        {
            RuleFor(recipe => recipe.Title).NotEmpty().WithMessage(ResourceMessagesException.TITLE_EMPTY);
            RuleFor(recipe => recipe.CookingTime).IsInEnum().WithMessage(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
            RuleFor(recipe => recipe.Difficulty).IsInEnum().WithMessage(ResourceMessagesException.DIFFICULTY_NOT_SUPPORTED);
            RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0).WithMessage(ResourceMessagesException.MUST_CONTAIN_ONE_INGREDIENT);
            RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0).WithMessage(ResourceMessagesException.MUST_CONTAIN_AN_INSTRUCTION);
            RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
            RuleForEach(recipe => recipe.Ingredients).NotEmpty().WithMessage(ResourceMessagesException.INGREDIENT_EMPTY);
            RuleForEach(recipe => recipe.Instructions).ChildRules(instructionRule =>
            {
                instructionRule.RuleFor(instruction => instruction.Step).GreaterThan(0).WithMessage(ResourceMessagesException.NO_NEGATIVE_INSTRUCTION_STEP);
                instructionRule
                    .RuleFor(instruction => instruction.Text)
                    .NotEmpty().WithMessage(ResourceMessagesException.INSTRUCTION_EMPTY)
                    .MaximumLength(2000).WithMessage(ResourceMessagesException.INSTRUCTION_EXCEEDS_MAXIMUM_SIZE);
            });
            RuleFor(recipe => recipe.Instructions)
                .Must(instructions => instructions.Select(i => i.Step).Distinct().Count() == instructions.Count)
                .WithMessage(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_HAVE_THE_SAME_ORDER);
        }
    }
}
