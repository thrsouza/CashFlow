using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Entities;
using CashFlow.Exeption.ExceptionsBase;
using CashFlow.Exeption;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetByIdExpenseUseCase : IGetByIdExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetByIdExpenseUseCase(
        IExpensesReadOnlyRepository repository,
        IMapper mapper)
    {
        this._repository = repository;
        this._mapper = mapper;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var result = await this._repository.GetById(id);

        if (result is null)
        {
            throw new CashFlowNotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        return this._mapper.Map<ResponseExpenseJson>(result);
    }
}
