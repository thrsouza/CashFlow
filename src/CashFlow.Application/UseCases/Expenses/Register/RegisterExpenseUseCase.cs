using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.AuthenticatedUser;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IExpensesWriteOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRegisterExpenseUseCase
{
    public async Task<ResponseRegisteredExpenseJson> Execute(RequestRegisterExpenseJson requestRegister)
    {
        Validate(requestRegister);
        
        var authenticatedUser = await authenticatedUserService.Get();

        var expense = mapper.Map<Expense>(requestRegister);
        expense.UserId = authenticatedUser.Id;

        await repository.Add(expense);

        await unitOfWork.Commit();

        var response = mapper.Map<ResponseRegisteredExpenseJson>(expense);

        return response;
    }

    private static void Validate(RequestRegisterExpenseJson requestRegister)
    {
        var validator = new RegisterExpenseValidator();

        var result = validator.Validate(requestRegister);

        if (result.IsValid) return;
        var errorMessages = result.Errors.Select(err => err.ErrorMessage).ToList();

        throw new CashFlowValidationErrorException(errorMessages: errorMessages);
    }
}
