using RecipeBook.Domain.Enums;

namespace RecipeBook.Domain.Entities;

public class Recipe : EntityBase
{
    public long UserId { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    public string Title { get; set; } = "";
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public IList<Ingredient> Ingredients { get; set; } = [];
    public IList<Instruction> Instructions { get; set; } = [];
    public string? ImageIndentifier { get; set; }
    public IList<DishType> DishTypes { get; set; } = [];
}
