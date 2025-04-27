using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Application.UserCases.User.Register;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestUserJsonBuilder.Build();

            var useCase = CreateUseCase();
            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Tokens.ShouldNotBeNull();
            result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
            result.Name.ShouldBe(request.Name);
        }

        [Fact]
        public async Task Erro_Email_Already_Registered()
        {
            var request = RequestUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            async Task act() => await useCase.Execute(request);

            var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.EMAIL_IN_USE);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestUserJsonBuilder.Build();
            request.Name = "";

            var useCase = CreateUseCase();

            async Task act() => await useCase.Execute(request);

            var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var readRepositoryBuild = new UserReadOnlyRepositoryBuilder();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();
            var accessToken = JwtTokenGeneratorBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();

            if (!string.IsNullOrEmpty(email))
                readRepositoryBuild.ExistActiveUserWithEmail(email);

            return new RegisterUserUseCase(readRepositoryBuild.Build(), writeRepository, unitOfWork, mapper, accessToken, passwordEncripter);
        }
    }
}
