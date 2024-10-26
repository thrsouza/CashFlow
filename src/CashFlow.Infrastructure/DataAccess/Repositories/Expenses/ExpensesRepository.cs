using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CashFlow.Infrastructure.DataAccess.Repositories.Expenses;

internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task<List<Expense>> GetAllByDate(DateTime startDate, DateTime endDate)
    {
        return await _dbContext.Expenses.AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expenses.AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id);
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);

        _dbContext.SaveChanges();
    }

    public void Update(Expense expense)
    {
        this._dbContext.Expenses.Update(expense);
    }

    public async Task<bool> Delete(long id)
    {
        var result = await this._dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

        if (result is not null)
        {
            this._dbContext.Expenses.Remove(result);

            return true;
        }

        return false;
    }
}
