using CommonTestUtilities.Entities;
using CommonTestUtilities.IdEncription;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Enums;
using RecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private RecipeBook.Domain.Entities.User _user = default!;
        private RecipeBook.Domain.Entities.Recipe _recipe = default!;
        private string _password = "";

        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;
        public string GetName() => _user.Name;
        public Guid GetUserIndentifier() => _user.UserIdentifier;

        public string GetRecipeTitle() => _recipe.Title;
        public Difficulty GetRecipeDifficulty() => _recipe.Difficulty!.Value;
        public CookingTime GetRecipeCookingTIme() => _recipe.CookingTime!.Value;
        public IList<DishType> GetDishTypes() => _recipe.DishTypes.Select(c => c.Type).ToList();
        public string GetRecipeEncriptedId()
        {
            var encripter = IdEncripterBuilder.Build();
            return encripter.Encode(_recipe.ID);
        }


        private void StartDatabase(RecipeBookDbContext dbContext)
        {
            (_user, _password) = UserBuilder.Build();
            _recipe = RecipeBuilder.Build(_user);

            dbContext.Users.Add(_user);
            dbContext.Recipes.Add(_recipe);
            dbContext.SaveChanges();
        }

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

                    StartDatabase(dbContext);
                });
        }
    }
}
