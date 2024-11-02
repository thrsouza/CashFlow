using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Expenses.GetById;

public class GetByIdExpenseUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var useCase = CreateUseCase(user, expense);

        // Act
        var result = await useCase.Execute(expense.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expense.Id);
        result.Title.Should().Be(expense.Title);
        result.Description.Should().Be(expense.Description);
        result.Date.Should().Be(expense.Date);
        result.Amount.Should().Be(expense.Amount);
        result.PaymentType.Should().Be((PaymentType)expense.PaymentType);
    }
    
    [Fact]
    public async Task Error_Expense_NotFound()
    {
        // Arrange
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        // Act
        var act = async () => await useCase.Execute(id: 1000);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowNotFoundException>();
        
        result.Where(exception => exception.GetErrors().Count == 1 && exception.GetErrors().Contains(ResourceErrorMessages.DataNotFound));
    }
    
    
    private static GetByIdExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var authenticatedUser = AuthenticatedUserServiceBuilder.Build(user);
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var mapper = MapperBuilder.Build();
        
        return new GetByIdExpenseUseCase(authenticatedUser, repository, mapper);
    }
}