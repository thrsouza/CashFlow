using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Expenses.Delete;

public class DeleteExpenseUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var useCase = CreateUseCase(user, expense);

        // Act
        var act = async () => await useCase.Execute(expense.Id);

        // Assert
        await act.Should().NotThrowAsync();
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
    
    
    private static IDeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var authenticatedUser = AuthenticatedUserServiceBuilder.Build(user);
        var expensesReadOnlyRepository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var expensesWriteOnlyRepository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new DeleteExpenseUseCase(authenticatedUser, expensesReadOnlyRepository, expensesWriteOnlyRepository, unitOfWork);
    }
}