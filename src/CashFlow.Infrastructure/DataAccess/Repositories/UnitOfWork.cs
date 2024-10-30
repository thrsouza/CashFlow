using CashFlow.Domain.Repositories;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UnitOfWork(CashFlowDbContext dbContext) : IUnitOfWork
{
    public async Task Commit() => await dbContext.SaveChangesAsync();
}
