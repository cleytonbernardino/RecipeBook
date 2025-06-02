using RecipeBook.Communication.Enums;

namespace RecipeBook.Communication.Responses
{
    public class ResponseGeneratedRecipeJson
    {
        public string Title { get; set; } = "";
        public IList<string> Ingredients { get; set; } = [];
        public IList<ResponseGeneratedInstructionJson> Instructions { get; set; } = [];
        public CookingTime CookingTime { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
