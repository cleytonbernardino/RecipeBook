using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Application.UserCases.User.Register;
using RecipeBook.Communiction.Requests;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();

            RegisterUserUseCase useCase = CreateUseCase();
            var result = await useCase.Execute(request);

            Assert.NotNull(result);
            Assert.NotNull(result.Tokens);
            Assert.NotEmpty(result.Tokens.AccessToken);
            Assert.Equal(result.Name, request.Name);
        }

        [Fact]
        public async Task Erro_Email_Already_Registered()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            
            RegisterUserUseCase useCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await useCase.Execute(request);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Single(exception.ErrorMessagens);
            Assert.Equal(exception.ErrorMessagens[0], ResourceMessagesException.EMAIL_IN_USE);       
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            RequestRegisterUserJson request = RequestUserJsonBuilder.Build();
            request.Name = "";

            RegisterUserUseCase useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Single(exception.ErrorMessagens);
            Assert.Equal(exception.ErrorMessagens[0], ResourceMessagesException.NAME_EMPTY);
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var readRepositoryBuild = new UserReadOnlyRepositoryBuilder();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWork.Build();
            var mapper = MapperBuilder.Build();
            var accessToken = JwtTokenGeneratorBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();

            if (!string.IsNullOrEmpty(email))
                readRepositoryBuild.ExistActiveUserWithEmail(email);

            return new RegisterUserUseCase(readRepositoryBuild.Build(), writeRepository, unitOfWork, mapper, accessToken, passwordEncripter);
        }
    }
}
