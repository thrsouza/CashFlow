using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Users.Profile;

public class GetUserProfileUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        
        // Act
        var result = await useCase.Execute();
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
    }
    
    private static IGetUserProfileUseCase CreateUseCase(User user)
    {
        var authenticatedUserService = AuthenticatedUserServiceBuilder.Build(user);
        var mapper = MapperBuilder.Build();
        
        return new GetUserProfileUseCase(authenticatedUserService, mapper);
    }
}