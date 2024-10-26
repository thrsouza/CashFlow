using CashFlow.Communication.Requests;
using CashFlow.Exeption;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses;

public class RequestExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    public RequestExpenseValidator()
    {
        RuleFor(request => request.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_IS_REQUIRED);
        RuleFor(request => request.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(request => request.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.DATE_CONNOT_BE_FOR_THE_FUTURE);
        RuleFor(request => request.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_IS_NOT_VALID);
    }
}