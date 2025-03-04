using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Infrastructure.DataAccess;
using RecipeBook.Infrastructure.DataAccess.Repositories;
using RecipeBook.Infrastructure.Extensions;
using RecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using System.Reflection;

namespace RecipeBook.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddJwtTokens(services, configuration);
            if (configuration.IsUnitTestEnviroment())
                return;

            AddDbContext(services, configuration);
            AddFluentMigrator(services, configuration);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.ConnectionString();
            MySqlServerVersion serverVersion = new(new Version(8, 0, 28));

            services.AddDbContext<RecipeBookDbContext>(dbContextOptions =>
                dbContextOptions.UseMySql(connectionString, serverVersion)
            );
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.ConnectionString();
            services.AddFluentMigratorCore().ConfigureRunner(options =>
                options
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure")).For.All()
            );
        }

        private static void AddJwtTokens(IServiceCollection services, IConfiguration configuration)
        {
            uint expirationTimeMinutes = uint.Parse(configuration.GetSection("Settings:JWT:ExpirationTimeMinutes").Value!);
            string SigningKey = configuration.GetSection("Settings:JWT:SigningKey").Value!;

            services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, SigningKey));
        }
    }
}
