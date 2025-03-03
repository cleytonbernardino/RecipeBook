using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Cryptography;
using RecipeBook.Application.Services.AutoMapper;
using RecipeBook.Application.UserCases.Login.DoLogin;
using RecipeBook.Application.UserCases.User.Register;

namespace RecipeBook.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddPasswordEncrypter(services, configuration);
            AddAutoMapper(services);
            AddUseCases(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping());
            }).CreateMapper());
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        }

        private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            string passwordSalt = configuration.GetValue<string>("Settings:Passwords:Salt") !;
            services.AddScoped(option => new PasswordEncripter(passwordSalt));
        }
    }
}
