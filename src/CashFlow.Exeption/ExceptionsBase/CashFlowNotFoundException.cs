using System.Net;

namespace CashFlow.Exeption.ExceptionsBase;

public class CashFlowNotFoundException(string message) : CashFlowException(message)
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [this.Message];
    }
}
