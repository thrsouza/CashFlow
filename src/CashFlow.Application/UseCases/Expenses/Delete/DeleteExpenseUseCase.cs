using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exeption.ExceptionsBase;
using CashFlow.Exeption;
using CashFlow.Domain.Repositories;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        this._repository = repository;
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public async Task Execute(long id)
    {
        var result = await this._repository.Delete(id);

        if (result)
        {
            await this._unitOfWork.Commit();

            return;
        }

        throw new CashFlowNotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }
}
