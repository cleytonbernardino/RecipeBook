using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBook.Domain.Entities
{
    [Table("DishTypes")]
    public class DishType : EntityBase
    {
        public long RecipeId { get; set; }
        public Enums.DishType Type { get; set; }
    }
}