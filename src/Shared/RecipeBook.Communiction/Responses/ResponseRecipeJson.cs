using RecipeBook.Communication.Enums;

namespace RecipeBook.Communication.Responses
{
    public class ResponseRecipeJson
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public CookingTime CookingTime { get; set; }
        public Difficulty? Difficulty { get; set; }
        public IList<ResponseIngredientsJson> Ingredients { get; set; } = [];
        public IList<ResponseInstructionJson> Instructions { get; set; } = [];
        public IList<DishType> DishTypes { get; set; } = [];
        public string? ImageUrl { get; set; } = null;
    }
}
