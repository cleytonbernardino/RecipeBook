namespace RecipeBook.Domain.Entities
{
    public class Recipe : EntityBase
    {
        public long UserId { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string Title { get; set; } = "";
        public Enums.CookingTime? CookingTime { get; set; }
        public Enums.Difficulty? Difficulty { get; set; }
        public IList<Ingredient> Ingredients { get; set; } = [];
        public IList<Instruction> Instructions { get; set; } = [];
        public IList<DishType> DishTypes { get; set; } = [];
    }
}
