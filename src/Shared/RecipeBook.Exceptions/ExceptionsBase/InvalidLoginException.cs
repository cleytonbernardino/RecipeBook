namespace RecipeBook.Exceptions.ExceptionsBase
{
    public class InvalidLoginException : RecipeBookException
    {
        public InvalidLoginException() : base(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID) { }
    }
}
