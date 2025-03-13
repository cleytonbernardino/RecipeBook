using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Application.UserCases.Login.DoLogin;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Cryptography;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, string password) = UserBuilder.Build();

            DoLoginUseCase useCase = createUseCase(user);

            ResponseResgisteredUserJson result = await useCase.Execute(new RequestLoginJson()
            {
                Email = user.Email,
                Password = password
            });

            Assert.NotNull(result);
            Assert.NotNull(result.Tokens);
            Assert.NotEmpty(result.Tokens.AccessToken);
            Assert.Equal(user.Name, result.Name);
        }

        [Fact]
        public async Task Error_Invalid_User()
        {
            RequestLoginJson request = RequestLoginJsonBuilder.Build();

            DoLoginUseCase useCase = createUseCase();

            Func<Task> act = async () => { await useCase.Execute(request); };

            var exception = await Assert.ThrowsAsync<InvalidLoginException>(act);
            Assert.Equal(exception.Message, ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
        }

        private static DoLoginUseCase createUseCase(RecipeBook.Domain.Entities.User? user = null)
        {
            IPasswordEncripter passwordEncripty = PasswordEncripterBuilder.Build();
            UserReadOnlyRepositoryBuilder readOnlyRepository = new();
            IAccessTokenGenerator accessToken = JwtTokenGeneratorBuilder.Build();

            if (user is not null)
                readOnlyRepository.GetByEmailAndPassword(user);

            return new DoLoginUseCase(readOnlyRepository.Build(), accessToken, passwordEncripty);
        }
    }
}
