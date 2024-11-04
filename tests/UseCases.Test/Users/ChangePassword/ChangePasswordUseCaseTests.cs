using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Users.ChangePassword;

public class ChangePasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        
        var request = RequestChangePasswordJsonBuilder.Build();
        
        var useCase = CreateUseCase(user, request.Password);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        // Arrange
        var user = UserBuilder.Build();
        
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;
        
        var useCase = CreateUseCase(user, request.Password);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowValidationErrorException>();
        
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.PasswordIsNotValid));
    }
    
    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        // Arrange
        var user = UserBuilder.Build();
        
        var request = RequestChangePasswordJsonBuilder.Build();
        
        var useCase = CreateUseCase(user);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowValidationErrorException>();
        
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.PasswordIncorrect));
    }
    
    private static IChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var authenticatedUserService = AuthenticatedUserServiceBuilder.Build(user);
        var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var passwordEncryptor = new PasswordEncryptorBuilder().Verify(password).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new ChangePasswordUseCase(authenticatedUserService, userUpdateOnlyRepository, passwordEncryptor, unitOfWork);
    }
}