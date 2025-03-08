namespace RecipeBook.Domain.Entities
{
    public class DishType
    {
        public long RecipeId { get; set; }
        public Enums.DishType Type { get; set; }
    }
}