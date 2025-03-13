using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBook.Domain.Entities
{
    [Table("Ingredients")]
    public class Ingredient : EntityBase
    {
        public long RecipeId { get; set; }
        public string Name { get; set; } = "";
    }
}
