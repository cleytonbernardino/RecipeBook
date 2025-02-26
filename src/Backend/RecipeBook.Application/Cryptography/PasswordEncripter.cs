using System.Security.Cryptography;
using System.Text;

namespace RecipeBook.Application.Cryptography
{
    public class PasswordEncripter
    {
        private readonly string _salt;

        public PasswordEncripter(string salt) => _salt = salt;

        public string Encript(string password)
        {
            string newPassword = password + _salt;

            byte[] bytes = Encoding.UTF8.GetBytes(newPassword);
            byte[] hashBytes = SHA512.HashData(bytes);
            return StringBytes(hashBytes);
        }

        private string StringBytes(byte[] bytes)
        {
            StringBuilder sb = new();
            foreach (Byte b in bytes)
            {
                string hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
