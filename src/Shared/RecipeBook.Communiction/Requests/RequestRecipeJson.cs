using RecipeBook.Communiction.Enums;

namespace RecipeBook.Communiction.Requests
{
    public class RequestRecipeJson
    {
        public string Title { get; set; } = "";
        public CookingTime? CookingTime { get; set; }
        public Difficulty? Difficulty { get; set; }
        public IList<string> Ingredients { get; set; } = [];
        public IList<RequestInstructionJson> Intructions { get; set; } = [];
        public IList<DishType> DishTypes { get; set; } = [];
    }
}
