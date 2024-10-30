using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

public class CashFlowInvalidLoginException() : CashFlowException(ResourceErrorMessages.CredentialsInvalid)
{
    public override int StatusCode { get; } = (int)HttpStatusCode.Unauthorized;
    public override List<string> GetErrors()
    {
        return [Message];
    }
}