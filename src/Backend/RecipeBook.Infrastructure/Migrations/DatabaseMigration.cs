using Dapper;
using MySqlConnector;

namespace RecipeBook.Infrastructure.Migrations
{
    public class DatabaseMigration
    {
        public static void Migrate(string connectionString)
        {
            EnsureDatabaseCreated(connectionString);
        }

        private static void EnsureDatabaseCreated(string connectionString)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.Database;
            connectionStringBuilder.Remove("Database");
            
            using MySqlConnection dbConnection = new(connectionStringBuilder.ConnectionString);

            dbConnection.Execute($"create database if not exists {databaseName};");
        }
    }
}
