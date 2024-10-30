using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetByIdExpenseUseCase(
    IExpensesReadOnlyRepository repository,
    IMapper mapper) : IGetByIdExpenseUseCase
{
    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var result = await repository.GetById(id);

        if (result is null)
        {
            throw new CashFlowNotFoundException();
        }

        return mapper.Map<ResponseExpenseJson>(result);
    }
}
