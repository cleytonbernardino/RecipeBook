using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Application.UserCases.Login.DoLogin;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, string password) = UserBuilder.Build();

            var useCase = createUseCase(user);

            var result = await useCase.Execute(new RequestLoginJson()
            {
                Email = user.Email,
                Password = password
            });

            result.ShouldNotBeNull();
            result.Tokens.ShouldNotBeNull();
            result.Tokens.AccessToken.ShouldNotBeEmpty();
            result.Name.ShouldBe(user.Name);
        }

        [Fact]
        public async Task Error_Invalid_User()
        {
            var request = RequestLoginJsonBuilder.Build();

            var useCase = createUseCase();

            async Task act() { await useCase.Execute(request); };

            var exception = await Assert.ThrowsAsync<InvalidLoginException>(act);

            exception.Message.ShouldBe(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
        }

        private static DoLoginUseCase createUseCase(RecipeBook.Domain.Entities.User? user = null)
        {
            var passwordEncripty = PasswordEncripterBuilder.Build();
            UserReadOnlyRepositoryBuilder readOnlyRepository = new();
            var accessToken = JwtTokenGeneratorBuilder.Build();

            if (user is not null)
                readOnlyRepository.GetByEmailAndPassword(user);

            return new DoLoginUseCase(readOnlyRepository.Build(), accessToken, passwordEncripty);
        }
    }
}
