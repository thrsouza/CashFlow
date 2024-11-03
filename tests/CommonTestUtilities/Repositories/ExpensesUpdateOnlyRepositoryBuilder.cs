using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ExpensesUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesUpdateOnlyRepository> _repository = new();
    
    public ExpensesUpdateOnlyRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense is not null)
        {
            _repository.Setup(x => x.GetById(expense.Id, user)).ReturnsAsync(expense);
        }
        
        return this;
    }
    
    public IExpensesUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }
}