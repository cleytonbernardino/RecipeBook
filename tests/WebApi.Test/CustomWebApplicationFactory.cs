using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private RecipeBook.Domain.Entities.User _user = default!;
        private string _password = "";

        public string GetEmail() => _user.Email;

        public string GetPassword() => _password;

        public string GetName() => _user.Name;

        public Guid GetUserIndentifier() => _user.UserIdentifier;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RecipeBookDbContext>));
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<RecipeBookDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });

                    using var scope = services.BuildServiceProvider().CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<RecipeBookDbContext>();
                    dbContext.Database.EnsureDeleted();

                    (_user, _password) = UserBuilder.Build();
                    dbContext.Users.Add(_user);
                    dbContext.SaveChanges();
                });
        }
    }
}
