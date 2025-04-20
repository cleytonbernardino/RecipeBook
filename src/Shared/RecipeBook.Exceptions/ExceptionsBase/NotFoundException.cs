namespace RecipeBook.Exceptions.ExceptionsBase
{
    public class NotFoundException : RecipeBookException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
