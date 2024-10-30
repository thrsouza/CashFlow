using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static async Task MigrateDatabaseAsync(IServiceProvider services)
    {
        await using var dbContext = services.GetRequiredService<CashFlowDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}