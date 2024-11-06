using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CashFlow.Infrastructure.DataAccess.Repositories.Expenses;

internal class ExpensesRepository(CashFlowDbContext dbContext) 
    : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    public async Task<List<Expense>> GetAll(User user)
    {
        return await dbContext.Expenses.AsNoTracking()
            .Where(expense => expense.UserId == user.Id).ToListAsync();
    }

    public async Task<List<Expense>> GetAllByDate(DateTime startDate, DateTime endDate, User user)
    {
        return await dbContext.Expenses.AsNoTracking()
            .Where(expense => expense.UserId == user.Id && expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id, User user)
    {
        return await GetFullExpense().AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id, User user)
    {
        return await GetFullExpense().FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    public async Task Add(Expense expense)
    {
        await dbContext.Expenses.AddAsync(expense);
    }

    public void Update(Expense expense)
    {
        dbContext.Expenses.Update(expense);
    }

    public async Task Delete(long id)
    {
        var result = await dbContext.Expenses.FirstAsync(expense => expense.Id == id);

        dbContext.Expenses.Remove(result);
    }

    private IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpense()
    {
        return dbContext.Expenses.Include(expense => expense.Tags);
    }
}
