using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users;

public class RequestUserValidator : AbstractValidator<RequestUserJson>
{
    public RequestUserValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage(ResourceErrorMessages.NameIsRequired);
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EmailIsRequired)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.EmailIsNotValid);

        RuleFor(r => r.Password)
            .SetValidator(new PasswordValidator<RequestUserJson>());
    }
}