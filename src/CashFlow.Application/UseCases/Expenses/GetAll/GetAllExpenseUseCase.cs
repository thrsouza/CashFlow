using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase(
    IExpensesReadOnlyRepository repository,
    IMapper mapper) 
    : IGetAllExpenseUseCase
{
    public async Task<ResponseExpensesJson> Execute()
    {
        var result = await repository.GetAll();

        return new ResponseExpensesJson
        {
            Data = mapper.Map<List<ResponseExpensePreviewJson>>(result)
        };
    }
}
