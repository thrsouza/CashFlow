using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase : IGetAllExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetAllExpenseUseCase(
        IExpensesReadOnlyRepository repository,
        IMapper mapper)
    {
        this._repository = repository;
        this._mapper = mapper;
    }

    public async Task<ResponseExpensesJson> Execute()
    {
        var result = await this._repository.GetAll();

        return new ResponseExpensesJson
        {
            Data = this._mapper.Map<List<ResponseExpensePreviewJson>>(result)
        };
    }
}
