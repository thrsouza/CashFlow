using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Users.Update;

public class UpdateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(user);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        await act.Should().NotThrowAsync();
        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        // Arrange
        var user = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase(user);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowValidationErrorException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NameIsRequired));
    }
    
    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        // Arrange
        var user = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(user, request.Email);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowValidationErrorException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EmailAlreadyRegistered));
    }

    private static IUpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        var authenticatedUserService = AuthenticatedUserServiceBuilder.Build(user);
        var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var userReadOnlyRepository = new UsersReadOnlyRepositoryBuilder().ExistActiveUserWithEmail(email ?? user.Email).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new UpdateUserUseCase(authenticatedUserService, userUpdateOnlyRepository, userReadOnlyRepository, unitOfWork);
    }
}