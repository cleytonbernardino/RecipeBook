using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.ChangePassword;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Cryptography;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, string password) = UserBuilder.Build();

            RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();
            request.Password = password;

            ChangePasswordUseCase useCase = CreateUseCase(user);
            await useCase.Execute(request);

            IPasswordEncripter passwordEncripter = PasswordEncripterBuilder.Build();
            string encriptedPassword = passwordEncripter.Encript(request.NewPassword);

            Assert.Equal(user.Password, encriptedPassword);
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

            ChangePasswordUseCase useCase = CreateUseCase(user);
            Func<Task> act = async () => await useCase.Execute(request);

            var exceptions = await Assert.ThrowsAsync<ErrorOnValidationException>(act);

            Assert.Single(exceptions.ErrorMessagens);

            IPasswordEncripter passwordEncripter = PasswordEncripterBuilder.Build();
            string encriptedPassword = passwordEncripter.Encript(request.NewPassword);

            Assert.NotEqual(user.Password, encriptedPassword);
        }

        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            (var user, string password) = UserBuilder.Build();

            RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();

            ChangePasswordUseCase useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            var exceptions = await Assert.ThrowsAsync<ErrorOnValidationException>(act);

            Assert.Single(exceptions.ErrorMessagens);

            IPasswordEncripter passwordEncripter = PasswordEncripterBuilder.Build();
            string encriptedPassword = passwordEncripter.Encript(request.NewPassword);

            Assert.NotEqual(user.Password, encriptedPassword);
        }

        private static ChangePasswordUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            ILoggedUser loggedUserInterface = LoggedUserBuilder.Build(user);
            IUserUpdateOnlyRepository repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            IPasswordEncripter passwordEncripter = PasswordEncripterBuilder.Build();
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();

            return new ChangePasswordUseCase(loggedUserInterface, repository, passwordEncripter, unitOfWork);
        }
    }
}
