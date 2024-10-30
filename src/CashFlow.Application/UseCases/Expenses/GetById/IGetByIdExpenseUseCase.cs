using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public interface IGetByIdExpenseUseCase
{
    Task<ResponseExpenseJson> Execute(long id);
}
