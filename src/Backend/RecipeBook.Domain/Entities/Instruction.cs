using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBook.Domain.Entities
{
    [Table("Instructions")]
    public class Instruction : EntityBase
    {
        public long RecipeId { get; set; }
        public int Step { get; set; }
        public string Text { get; set; } = "";
    }
}
