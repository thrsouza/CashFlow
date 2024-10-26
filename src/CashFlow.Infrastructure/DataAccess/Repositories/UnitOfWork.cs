using CashFlow.Domain.Repositories;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly CashFlowDbContext _dbContext;

    public UnitOfWork(CashFlowDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task Commit() => await this._dbContext.SaveChangesAsync();
}
