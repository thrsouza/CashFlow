using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CashFlow.Infrastructure.DataAccess;

internal class CashFlowDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Expense> Expenses { get; set; }
}
