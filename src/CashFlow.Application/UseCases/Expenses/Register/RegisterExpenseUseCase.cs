using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exeption.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        this._repository = repository;
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public async Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request)
    {
        Validade(request);

        var entity = this._mapper.Map<Expense>(request);

        await this._repository.Add(entity);

        await this._unitOfWork.Commit();

        var response = this._mapper.Map<ResponseRegisteredExpenseJson>(entity);

        return response;
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
