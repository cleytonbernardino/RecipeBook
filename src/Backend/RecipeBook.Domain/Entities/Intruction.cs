namespace RecipeBook.Domain.Entities
{
    public class Intruction
    {
        public long RecipeId { get; set; }
        public string Step { get; set; } = "";
        public string Text { get; set; } = "";
    }
}
