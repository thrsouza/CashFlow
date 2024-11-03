using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Expenses.Register;

public class RegisterExpenseUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        
        var useCase = CreateUseCase(user);
        
        // Act
        var result = await useCase.Execute(request);
        
        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }
    
    [Fact]
    public async Task Error_Title_Empty()
    {
        // Arrange
        var user = UserBuilder.Build();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var useCase = CreateUseCase(user);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowValidationErrorException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TitleIsRequired));    
    }
    
    private static IRegisterExpenseUseCase CreateUseCase(User user)
    {
        var authenticatedUser = AuthenticatedUserServiceBuilder.Build(user);
        var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        
        return new RegisterExpenseUseCase(authenticatedUser, repository, unitOfWork, mapper);
    }
}