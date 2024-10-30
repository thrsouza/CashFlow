using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Exception;
using CashFlow.Domain.Repositories;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(
    IExpensesWriteOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IDeleteExpenseUseCase
{
    private readonly IMapper _mapper = mapper;

    public async Task Execute(long id)
    {
        var result = await repository.Delete(id);

        if (!result) throw new CashFlowNotFoundException(ResourceErrorMessages.ExpenseNotFound);
        
        await unitOfWork.Commit();
    }
}
