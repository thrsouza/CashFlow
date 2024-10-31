using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users;

public class RequestRegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RequestRegisterUserValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage(ResourceErrorMessages.NameIsRequired);
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EmailIsRequired)
            .EmailAddress()
            .When(r => !string.IsNullOrWhiteSpace(r.Email), applyConditionTo: ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EmailIsNotValid);

        RuleFor(r => r.Password)
            .SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}