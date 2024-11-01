using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesReadOnlyRepository
{
    Task<List<Expense>> GetAll(User user);
    Task<Expense?> GetById(long id, User user);
    Task<List<Expense>> GetAllByDate(DateTime startDate, DateTime endDate, User user);
}
