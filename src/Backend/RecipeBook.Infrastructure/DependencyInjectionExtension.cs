using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mscc.GenerativeAI;
using RecipeBook.Application.Cryptography;
using RecipeBook.Domain.Cryptography;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.OpenAI;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Infrastructure.DataAccess;
using RecipeBook.Infrastructure.DataAccess.Repositories;
using RecipeBook.Infrastructure.Extensions;
using RecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using RecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using RecipeBook.Infrastructure.Services.Google;
using RecipeBook.Infrastructure.Services.LoggedUser;
using RecipeBook.Infrastructure.Services.Storage;
using System.Reflection;

namespace RecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddPasswordEncrypter(services, configuration);
        AddJwtTokens(services, configuration);
        AddLoggedUser(services);
        if (configuration.IsUnitTestEnviroment())
            return;

        AddDbContext(services, configuration);
        AddFluentMigrator(services, configuration);
        AddGoogleGemini(services, configuration);
        string localBlob = configuration.GetSection("Settings:SaveImagemLocal").Value!;
        if (localBlob == "false")
            AddAzureStorageService(services, configuration);
        else
            AddLocalBlobService(services);
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
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
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

    private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
    {
        string passwordSalt = configuration.GetSection("Settings:Passwords:Salt").Value!;
        services.AddScoped<IPasswordEncripter>(option => new Sha512Encripter(passwordSalt));
    }

    private static void AddJwtTokens(IServiceCollection services, IConfiguration configuration)
    {
        uint expirationTimeMinutes = uint.Parse(configuration.GetSection("Settings:JWT:ExpirationTimeMinutes").Value!);
        string signingKey = configuration.GetSection("Settings:JWT:SigningKey").Value!;

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(expirationTimeMinutes, signingKey));
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddGoogleGemini(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGenerateRecipeAI, GenerateRecipeAI>();

        string key = configuration.GetSection("Settings:Google:GeminiApiToken").Value!;

        services.AddScoped<IGenerativeAI>(options => new GoogleAI(apiKey: key));
    }

    private static void AddAzureStorageService(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetSection("Settings:Blob:Azure").Value!;

        if ( !string.IsNullOrEmpty(connectionString) )
            services.AddScoped<IBlobStorageService>(c => new AzureStorageService(new BlobServiceClient(connectionString)));
    }

    private static void AddLocalBlobService(IServiceCollection services)
    {
        services.AddScoped<IBlobStorageService, LocalStorageService>();
    }
}
