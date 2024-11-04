using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Users.Delete;

public class DeleteUserAccountUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        // Act
        var act = async () => await useCase.Execute();  

        // Assert
        await act.Should().NotThrowAsync();
    }

    private static IDeleteUserAccountUseCase CreateUseCase(User user)
    {
        var authenticatedUserService = AuthenticatedUserServiceBuilder.Build(user);
        var usersWriteOnlyRepository = UsersWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteUserAccountUseCase(authenticatedUserService, usersWriteOnlyRepository, unitOfWork);
    }
}