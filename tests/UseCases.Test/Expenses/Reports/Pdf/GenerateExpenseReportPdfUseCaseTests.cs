using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;
using PdfSharp.Fonts;

namespace UseCases.Test.Expenses.Reports.Pdf;

public class GenerateExpenseReportPdfUseCaseTests
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
    
    
    private static IGenerateExpensesReportPdfUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        GlobalFontSettings.FontResolver = new ExpensesReportPdfFontResolver();
        
        var authenticatedUser = AuthenticatedUserServiceBuilder.Build(user);
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetAllByDate(user, expenses).Build();
        
        return new GenerateExpensesReportPdfUseCase(authenticatedUser, repository);
    }
}