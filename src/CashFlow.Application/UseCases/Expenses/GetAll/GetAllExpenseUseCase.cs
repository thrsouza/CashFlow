using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.AuthenticatedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IExpensesReadOnlyRepository repository,
    IMapper mapper) 
    : IGetAllExpenseUseCase
{
    public async Task<ResponseExpensesJson> Execute()
    {
        var authenticatedUser = await authenticatedUserService.Get();
        
        var result = await repository.GetAll(authenticatedUser);

        return new ResponseExpensesJson
        {
            Data = mapper.Map<List<ResponseExpensePreviewJson>>(result)
        };
    }
}
