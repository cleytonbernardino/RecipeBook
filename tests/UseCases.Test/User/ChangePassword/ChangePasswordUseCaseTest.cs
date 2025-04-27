using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.ChangePassword;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, string password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = password;

            var useCase = CreateUseCase(user);
            await useCase.Execute(request);

            var passwordEncripter = PasswordEncripterBuilder.Build();
            string encriptedPassword = passwordEncripter.Encript(request.NewPassword);

            encriptedPassword.ShouldBe(user.Password);
        }

        [Fact]
        public async Task Error_NewPassword_Empty()
        {
            (var user, string password) = UserBuilder.Build();

            RequestChangePasswordJson request = new()
            {
                Password = password,
                NewPassword = ""
            };

            var useCase = CreateUseCase(user);
            async Task act() => await useCase.Execute(request);

            var exceptions = await act().ShouldThrowAsync<ErrorOnValidationException>();

            exceptions.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);

            var passwordEncripter = PasswordEncripterBuilder.Build();
            string encriptedPassword = passwordEncripter.Encript(request.NewPassword);

            exceptions.ErrorMessagens.ShouldHaveSingleItem().ShouldNotBe(encriptedPassword);
        }

        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            (var user, string password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            async Task act() => await useCase.Execute(request);

            var exceptions = await act().ShouldThrowAsync<ErrorOnValidationException>();

            var passwordEncripter = PasswordEncripterBuilder.Build();
            string encriptedPassword = passwordEncripter.Encript(request.NewPassword);

            exceptions.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.PASSWORD_DIFFERENT_FROM_CURRENT_PASSWORD);
            encriptedPassword.ShouldNotBe(user.Password);
        }

        private static ChangePasswordUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            var loggedUserInterface = LoggedUserBuilder.Build(user);
            var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new ChangePasswordUseCase(loggedUserInterface, repository, passwordEncripter, unitOfWork);
        }
    }
}
