using AutoMapper;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.AuthenticatedUser;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IExpensesReadOnlyRepository expensesReadOnlyRepository,
    IExpensesWriteOnlyRepository expensesWriteOnlyRepository,
    IUnitOfWork unitOfWork)
    : IDeleteExpenseUseCase
{

    public async Task Execute(long id)
    {
        var authenticatedUser = await authenticatedUserService.Get();

        var result = await expensesReadOnlyRepository.GetById(id, authenticatedUser);
        
        if (result is null) throw new CashFlowNotFoundException();
        
        await expensesWriteOnlyRepository.Delete(id);
        
        await unitOfWork.Commit();
    }
}
