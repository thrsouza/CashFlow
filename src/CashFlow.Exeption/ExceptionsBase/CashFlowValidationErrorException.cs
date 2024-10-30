using System.Net;

namespace CashFlow.Exeption.ExceptionsBase;

public class CashFlowValidationErrorException(List<string> errorMessages) : CashFlowException(string.Empty)
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
       return errorMessages;
    }
}
