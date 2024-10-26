using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public interface IGetByIdExpenseUseCase
{
    Task<ResponseExpenseJson> Execute(long id);
}
