using System.Text.RegularExpressions;
using CashFlow.Exception;
using FluentValidation;
using FluentValidation.Validators;

namespace CashFlow.Application.UseCases.Users;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string ErrorMessageKey = "ErrorMessage";

    public override string Name => "PasswordValidator";


    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ErrorMessageKey}}}";
    }
    
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.PasswordIsNotValid);
            return false;
        }

        if (password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.PasswordIsNotValid);
            return false;
        }
        if (UpperCaseLetter().IsMatch(password) == false)
        {
            context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.PasswordIsNotValid);
            return false;
        }

        if (LowerCaseLetter().IsMatch(password) == false)
        {
            context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.PasswordIsNotValid);
            return false;
        }

        if (Numbers().IsMatch(password) == false)
        {
            context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.PasswordIsNotValid);
            return false;
        }

        if (SpecialSymbols().IsMatch(password) == false)
        {
            context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.PasswordIsNotValid);
            return false;
        }

        return true;
    }

    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();
    [GeneratedRegex(@"[a-z]+")]
    private static partial Regex LowerCaseLetter();
    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex Numbers();
    [GeneratedRegex(@"[\!\?\*\.]+")]
    private static partial Regex SpecialSymbols();
}