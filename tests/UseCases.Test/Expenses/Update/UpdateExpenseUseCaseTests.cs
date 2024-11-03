using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;

namespace UseCases.Test.Expenses.Update;

public class UpdateExpenseUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        var request = RequestRegisterExpenseJsonBuilder.Build();

        var useCase = CreateUseCase(user, expense);

        // Act
        var act = async () => await useCase.Execute(expense.Id, request);

        // Assert
        await act.Should().NotThrowAsync();
        
        expense.Title.Should().Be(request.Title);
        expense.Description.Should().Be(request.Description);
        expense.Date.Should().Be(request.Date);
        expense.Amount.Should().Be(request.Amount);
        expense.PaymentType.Should().Be((PaymentType)request.PaymentType);
        expense.UserId.Should().Be(user.Id);
    }
    
    [Fact]
    public async Task Error_Title_Empty()
    {
        // Arrange
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user, expense);

        // Act
        var act = async () => await useCase.Execute(expense.Id, request);

        // Assert
        var result =await act.Should().ThrowAsync<CashFlowValidationErrorException>();
        
        result.Where(exception => exception.GetErrors().Count == 1 && exception.GetErrors().Contains(ResourceErrorMessages.TitleIsRequired));
    }
    
    [Fact]
    public async Task Error_Expense_NotFound()
    {
        // Arrange
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        var request = RequestRegisterExpenseJsonBuilder.Build();

        // Act
        var act = async () => await useCase.Execute(id: 1000, request);
        
        // Assert
        var result = await act.Should().ThrowAsync<CashFlowNotFoundException>();
        
        result.Where(exception => exception.GetErrors().Count == 1 && exception.GetErrors().Contains(ResourceErrorMessages.DataNotFound));
    }
    
    private static IUpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var authenticatedUser = AuthenticatedUserServiceBuilder.Build(user);
        var repository = new ExpensesUpdateOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        
        return new UpdateExpenseUseCase(authenticatedUser, repository, unitOfWork, mapper);
    }
}