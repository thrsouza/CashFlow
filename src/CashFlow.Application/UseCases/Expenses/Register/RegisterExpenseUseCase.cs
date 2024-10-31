using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase(
    IExpensesWriteOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRegisterExpenseUseCase
{
    public async Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request)
    {
        Validate(request);

        var entity = mapper.Map<Expense>(request);
        entity.UserId = 1;

        await repository.Add(entity);

        await unitOfWork.Commit();

        var response = mapper.Map<ResponseRegisteredExpenseJson>(entity);

        return response;
    }

    private static void Validate(RequestExpenseJson request)
    {
        var validator = new RegisterExpenseValidator();

        var result = validator.Validate(request);

        if (result.IsValid) return;
        var errorMessages = result.Errors.Select(err => err.ErrorMessage).ToList();

        throw new CashFlowValidationErrorException(errorMessages: errorMessages);
    }
}
