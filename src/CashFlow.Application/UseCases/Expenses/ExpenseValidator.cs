﻿using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses;

public class ExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    public ExpenseValidator()
    {
        RuleFor(request => request.Title).NotEmpty().WithMessage(ResourceErrorMessages.TitleIsRequired);
        RuleFor(request => request.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AmountMustBeGreaterThanZero);
        RuleFor(request => request.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.DateCannotBeForTheFuture);
        RuleFor(request => request.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PaymentTypeIsNotValid);
        RuleFor(request => request.Tags).ForEach(rule => rule.IsInEnum().WithMessage(ResourceErrorMessages.TagTypeIsNotValid));
    }
}