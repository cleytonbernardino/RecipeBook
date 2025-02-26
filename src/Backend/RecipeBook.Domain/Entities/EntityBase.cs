namespace RecipeBook.Domain.Entities
{
    public class EntityBase
    {
        public long ID { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
