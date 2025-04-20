using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Services.AutoMapper;
using RecipeBook.Application.UserCases.Login.DoLogin;
using RecipeBook.Application.UserCases.Recipe;
using RecipeBook.Application.UserCases.Recipe.Delete;
using RecipeBook.Application.UserCases.Recipe.Filter;
using RecipeBook.Application.UserCases.Recipe.GetById;
using RecipeBook.Application.UserCases.User.ChangePassword;
using RecipeBook.Application.UserCases.User.Profile;
using RecipeBook.Application.UserCases.User.Register;
using RecipeBook.Application.UserCases.User.Update;
using Sqids;

namespace RecipeBook.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services);
            AddIdEncoder(services, configuration);
            AddUseCases(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOpt =>
            {
                var sqids = option.GetService<SqidsEncoder<long>>()!;
                autoMapperOpt.AddProfile(new AutoMapping(sqids));
            }).CreateMapper());
        }

        private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
        {
            SqidsEncoder<long> sqids = new(new()
            {
                MinLength = 3,
                Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
            });

            services.AddSingleton(sqids);
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IGetUserProfile, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IRecipeUseCase, RecipeUseCase>();
            services.AddScoped<IGetRecipeByIdUseCase, GetRecipeByIdUseCase>();
            services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
            services.AddScoped<IDeleteRecipeUseCase, DeleteRecipeUseCase>();
        }
    }
}
