using System.Net;

namespace CashFlow.Exeption.ExceptionsBase;

public class CashFlowValidationErrorException : CashFlowException
{
    private readonly List<string> _errors;

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public CashFlowValidationErrorException(List<string> errorMessages) : base(string.Empty)
    {
        this._errors = errorMessages;
    }

    public override List<string> GetErrors()
    {
       return this._errors;
    }
}
