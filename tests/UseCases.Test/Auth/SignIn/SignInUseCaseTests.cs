using CashFlow.Application.UseCases.Auth.SignIn;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;

namespace UseCases.Test.Auth.SignIn;

public class SignInUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        
        var request = RequestSignInJsonBuilder.Build();
        request.Email = user.Email;
        
        var useCase = CreateUseCase(user, request.Password);
        
        // Act
        var result = await useCase.Execute(request);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Error_User_Not_Found()
    {
        // Arrange
        var user = UserBuilder.Build();
        var request = RequestSignInJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowInvalidLoginException>();
    }
    
    [Fact]
    public async Task Error_Password_Not_Match()
    {
        // Arrange
        var user = UserBuilder.Build();
        
        var request = RequestSignInJsonBuilder.Build();
        request.Email = user.Email;
        
        var useCase = CreateUseCase(user);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowInvalidLoginException>();
    }
    
    private static ISignInUseCase CreateUseCase(User user, string? password = null)
    {
        var userReadOnlyRepository = new UsersReadOnlyRepositoryBuilder().GetByEmail(user).Build();
        var passwordEncryptor = new PasswordEncryptorBuilder().Verify(password).Build();
        var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();
        
        return new SignInUseCase(userReadOnlyRepository, passwordEncryptor, accessTokenGenerator);
    }
}