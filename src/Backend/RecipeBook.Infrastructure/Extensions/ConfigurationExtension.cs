using Microsoft.Extensions.Configuration;

namespace RecipeBook.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static string ConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("Connection")!;

        public static bool IsUnitTestEnviroment(this IConfiguration configuration)
        {
            string? inMemoryTest = configuration.GetSection("InMemoryTest").Value;
            if (string.IsNullOrEmpty(inMemoryTest))
                return false;
            return true;
        }
    }
}

