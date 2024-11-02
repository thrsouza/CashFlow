using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ExpensesReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesReadOnlyRepository> _repository = new();
    
    public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
    {
        _repository.Setup(x => x.GetAll(user)).ReturnsAsync(expenses);
        
        return this;
    }
    
    public IExpensesReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}