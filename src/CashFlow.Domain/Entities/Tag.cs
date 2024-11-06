using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;

public class Tag
{
    public long Id { get; set; }
    public ExpenseTag Value { get; set; }
    
    public long ExpenseId { get; set; }
    public Expense Expense { get; set; } = default!;
}