using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exeption;
using CashFlow.Exeption.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;
public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IExpensesUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateExpenseUseCase(
        IExpensesUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        this._repository = repository;
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validade(request);

        var entity = await this._repository.GetById(id);

        if (entity is null)
        {
            throw new CashFlowNotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        this._mapper.Map(request, entity);

        this._repository.Update(entity);

        await this._unitOfWork.Commit();
    }

    private void Validade(RequestExpenseJson request)
    {
        var validator = new RequestExpenseValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(err => err.ErrorMessage).ToList();

            throw new CashFlowValidationErrorException(errorMessages: errorMessages);
        }
    }
}
