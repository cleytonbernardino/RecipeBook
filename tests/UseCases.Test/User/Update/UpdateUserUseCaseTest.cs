using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.Update;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build().user;

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            UpdateUserUseCase useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act();
            Assert.Equal(user.Name, request.Name);
            Assert.Equal(user.Email, request.Email);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var user = UserBuilder.Build().user;
            string originalName = user.Name;

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Name = "";

            UpdateUserUseCase useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Single(exception.ErrorMessagens);
            Assert.Equal(ResourceMessagesException.NAME_EMPTY, exception.ErrorMessagens[0]);
        }

        [Fact]
        public async Task Erro_Email_Empty()
        {
            var user = UserBuilder.Build().user;
            string originalName = user.Name;

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "";

            UpdateUserUseCase useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Single(exception.ErrorMessagens);
            Assert.Equal(ResourceMessagesException.EMAIL_EMPTY, exception.ErrorMessagens[0]);
        }

        [Fact]
        public async Task Erro_Email_Invalid()
        {
            var user = UserBuilder.Build().user;
            string originalName = user.Name;

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "email.com";

            UpdateUserUseCase useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Single(exception.ErrorMessagens);
            Assert.Equal(ResourceMessagesException.EMAIL_INVALID, exception.ErrorMessagens[0]);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var user = UserBuilder.Build().user;

            RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
            UpdateUserUseCase useCase = CreateUseCase(user, request.Email);

            Func<Task> act = async () => await useCase.Execute(request);

            // Errors
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Single(exception.ErrorMessagens);
            Assert.Equal(ResourceMessagesException.EMAIL_IN_USE, exception.ErrorMessagens[0]);

            // Validation
            Assert.NotEqual(user.Name, request.Name);
            Assert.NotEqual(user.Email, request.Email);
        }

        private static UpdateUserUseCase CreateUseCase(RecipeBook.Domain.Entities.User user, string? email = null)
        {
            ILoggedUser loggedUser = LoggedUserBuilder.Build(user);
            UserReadOnlyRepositoryBuilder userReadOnly = new UserReadOnlyRepositoryBuilder();
            IUserUpdateOnlyRepository userWriteOnly = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();

            if (!string.IsNullOrEmpty(email))
                userReadOnly.ExistActiveUserWithEmail(email);

            return new UpdateUserUseCase(loggedUser, userReadOnly.Build(), userWriteOnly, unitOfWork);

        }
    }
}
