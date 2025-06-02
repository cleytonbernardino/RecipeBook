using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Services.OpenAI;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UserCases.Recipe.Generate
{
    public class GenerateRecipeUseCase : IGenerateRecipeUseCase
    {
        private readonly IGenerateRecipeAI _generate;

        public GenerateRecipeUseCase(IGenerateRecipeAI generate)
        {
            _generate = generate;
        }

        public async Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request)
        {
            Validate(request);

            var response = await _generate.Generate(request.Ingredients);

            return new ResponseGeneratedRecipeJson()
            {
                Title = response.Title,
                Ingredients = response.Ingredients,
                CookingTime = (Communication.Enums.CookingTime)response.CookingTime,
                Instructions = response.Instructions.Select(c => new ResponseGeneratedInstructionJson
                {
                    Step = c.Step,
                    Text = c.Text
                }).ToList(),
                Difficulty = Communication.Enums.Difficulty.Easy
            };

        }

        private static void Validate(RequestGenerateRecipeJson request)
        {
            GenerateRecipeValidator validator = new();
            var result = validator.Validate(request);

            if ( !result.IsValid )
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).Distinct().ToArray();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
