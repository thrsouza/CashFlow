using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CashFlow.Infrastructure.DataAccess.Repositories.Expenses;

internal class ExpensesRepository(CashFlowDbContext dbContext) 
    : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    public async Task<List<Expense>> GetAll()
    {
        return await dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task<List<Expense>> GetAllByDate(DateTime startDate, DateTime endDate)
    {
        return await dbContext.Expenses.AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id)
    {
        return await dbContext.Expenses.AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id);
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id)
    {
        return await dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public async Task Add(Expense expense)
    {
        await dbContext.Expenses.AddAsync(expense);

        dbContext.SaveChanges();
    }

    public void Update(Expense expense)
    {
        dbContext.Expenses.Update(expense);
    }

    public async Task<bool> Delete(long id)
    {
        var result = await dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

        if (result is null) return false;
        
        dbContext.Expenses.Remove(result);

        return true;

    }
}
