using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var useCase = CreateUseCase();
        
        // Act
        var result = await useCase.Execute(request);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Error_Name_Empty()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase();
        
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
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(request.Email);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowValidationErrorException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EmailAlreadyRegistered));
    }

    private static IRegisterUserUseCase CreateUseCase(string? email = null)
    {
        var userReadOnlyRepositoryBuilder = new UsersReadOnlyRepositoryBuilder();
        if (!string.IsNullOrWhiteSpace(email))
        {
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);
        }

        var userReadOnlyRepository = userReadOnlyRepositoryBuilder.Build();
        var userWriteOnlyRepository = UsersWriteOnlyRepositoryBuilder.Build();
        var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();
        var passwordEncryptor = new PasswordEncryptorBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        
        return new RegisterUserUseCase(userReadOnlyRepository, userWriteOnlyRepository, accessTokenGenerator, passwordEncryptor, unitOfWork, mapper);
    }
}