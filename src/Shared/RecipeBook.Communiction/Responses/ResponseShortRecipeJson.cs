namespace RecipeBook.Communication.Responses;

public class ResponseShortRecipeJson
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public int AmountIngredients { get; set; }
    public string? ImageUrl { get; set; }
}
