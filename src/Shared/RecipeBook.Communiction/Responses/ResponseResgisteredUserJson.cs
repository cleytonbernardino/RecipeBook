namespace RecipeBook.Communiction.Responses
{
    public class ResponseResgisteredUserJson
    {
        public string Name { get; set; } = "";
        public ResponseTokenJson Tokens { get; set; } = default!;
    }
}
