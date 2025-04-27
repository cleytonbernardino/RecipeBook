using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using RecipeBook.Application.UserCases.User.Update;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build().user;

            var request = RequestUpdateUserJsonBuilder.Build();
            var useCase = CreateUseCase(user);

            async Task act() => await useCase.Execute(request);

            await act().ShouldNotThrowAsync();

            request.Name.ShouldBe(user.Name);
            request.Email.ShouldBe(user.Email);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var user = UserBuilder.Build().user;

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = "";

            var useCase = CreateUseCase(user);

            async Task act() => await useCase.Execute(request);

            var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public async Task Erro_Email_Empty()
        {
            var user = UserBuilder.Build().user;

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "";

            var useCase = CreateUseCase(user);

            async Task act() => await useCase.Execute(request);

            var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.EMAIL_EMPTY);
        }

        [Fact]
        public async Task Erro_Email_Invalid()
        {
            var user = UserBuilder.Build().user;

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "email.com";

            var useCase = CreateUseCase(user);

            async Task act() => await useCase.Execute(request);

            var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.EMAIL_INVALID);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var user = UserBuilder.Build().user;

            var request = RequestUpdateUserJsonBuilder.Build();
            var useCase = CreateUseCase(user, request.Email);

            async Task act() => await useCase.Execute(request);

            // Errors
            var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessagens.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.EMAIL_IN_USE);

            // Validation
            request.Name.ShouldNotBe(user.Name);
            request.Email.ShouldNotBe(user.Email);
        }

        private static UpdateUserUseCase CreateUseCase(RecipeBook.Domain.Entities.User user, string? email = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var userReadOnly = new UserReadOnlyRepositoryBuilder();
            var userWriteOnly = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            if (!string.IsNullOrEmpty(email))
                userReadOnly.ExistActiveUserWithEmail(email);

            return new UpdateUserUseCase(loggedUser, userReadOnly.Build(), userWriteOnly, unitOfWork);

        }
    }
}
