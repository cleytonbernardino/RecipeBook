namespace RecipeBook.Communiction.Responses
{
    public class ResponseErrorJson
    {
        public IList<string> Errors { get; set; }

        public ResponseErrorJson(IList<string> errors) => Errors = errors;

        public ResponseErrorJson(string erro) => Errors = new List<string> { erro };

        public bool TokenIsExpired { get; set; } = false;
    }
}
