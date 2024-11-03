using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Expenses.GetAll;

public class GetAllExpenseUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        var expenses = ExpenseBuilder.BuildList(user);

        var useCase = CreateUseCase(user, expenses);

        // Act
        var result = await useCase.Execute();

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNullOrEmpty().And.AllSatisfy(expense =>
        {
            expense.Id.Should().BeGreaterThan(0);
            expense.Title.Should().NotBeNullOrWhiteSpace();
            expense.Amount.Should().BeGreaterThan(0);
        });
    }
    
    private static IGetAllExpenseUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var authenticatedUser = AuthenticatedUserServiceBuilder.Build(user);
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
        var mapper = MapperBuilder.Build();
        
        return new GetAllExpenseUseCase(authenticatedUser, repository, mapper);
    }
}