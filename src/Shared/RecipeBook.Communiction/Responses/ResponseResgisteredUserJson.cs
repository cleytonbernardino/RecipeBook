namespace RecipeBook.Communication.Responses
{
    public class ResponseResgisteredUserJson
    {
        public string Name { get; set; } = "";
        public ResponseTokenJson Tokens { get; set; } = default!;
    }
}
