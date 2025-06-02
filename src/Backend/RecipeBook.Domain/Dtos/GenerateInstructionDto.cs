namespace RecipeBook.Domain.Dtos
{
    public class GenerateInstructionDto
    {
        public int Step { get; init; }
        public string Text { get; init; } = "";
    }
}
