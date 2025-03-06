namespace RecipeBook.Domain.Cryptography
{
    public interface IPasswordEncripter
    {
        public string Encript(string password);
    }
}
