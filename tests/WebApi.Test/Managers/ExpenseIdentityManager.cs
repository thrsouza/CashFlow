using CashFlow.Domain.Entities;

namespace WebApi.Test.Managers;

public class ExpenseIdentityManager(Expense expense)
{
    public long GetId() => expense.Id;
    
    public DateTime GetDate() => expense.Date;
}