namespace RecipeBook.Communication.Requests
{
    public class RequestChangePasswordJson
    {
        public string Password { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }
}
