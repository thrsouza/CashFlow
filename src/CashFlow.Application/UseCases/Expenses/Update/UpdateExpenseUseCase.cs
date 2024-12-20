﻿using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.AuthenticatedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;
public class UpdateExpenseUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IExpensesUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IUpdateExpenseUseCase
{
    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);
        
        var authenticatedUser = await authenticatedUserService.Get();

        var expense = await repository.GetById(id, authenticatedUser);

        if (expense is null)
        {
            throw new CashFlowNotFoundException();
        }

        expense.Tags.Clear();

        mapper.Map(request, expense);

        repository.Update(expense);

        await unitOfWork.Commit();
    }

    private static void Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();

        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(err => err.ErrorMessage).ToList();

        throw new CashFlowValidationErrorException(errorMessages: errorMessages);
    }
}
