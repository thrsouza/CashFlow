using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Expenses.Reports.Excel;

public class GenerateExpenseReportExcelUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        var expenses = ExpenseBuilder.BuildList(user);
        var useCase = CreateUseCase(user, expenses);
        
        // Act
        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));
        
        // Assert
        result.Should().NotBeNullOrEmpty();
    }   
    
    [Fact]
    public async Task Success_Empty()
    {
        // Arrange
        var user = UserBuilder.Build();
        var expenses = new List<Expense>();
        var useCase = CreateUseCase(user, expenses);
        
        // Act
        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));
        
        // Assert
        result.Should().BeNullOrEmpty();
    }
    
    
    private static IGenerateExpensesReportExcelUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var authenticatedUser = AuthenticatedUserServiceBuilder.Build(user);
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetAllByDate(user, expenses).Build();
        
        return new GenerateExpensesReportExcelUseCase(authenticatedUser, repository);
    }
}