namespace RecipeBook.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : RecipeBookException
    {
        public IList<string> ErrorMessagens { get; set; }

        public ErrorOnValidationException(IList<string> errorMessagens)
        {
            ErrorMessagens = errorMessagens;
        }
    }
}
