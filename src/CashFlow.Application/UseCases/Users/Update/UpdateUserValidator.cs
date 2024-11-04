using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage(ResourceErrorMessages.NameIsRequired);
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EmailIsRequired)
            .EmailAddress()
            .When(r => !string.IsNullOrWhiteSpace(r.Email), applyConditionTo: ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EmailIsNotValid);
    }
}