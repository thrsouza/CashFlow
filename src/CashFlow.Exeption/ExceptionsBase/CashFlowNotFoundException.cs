using System.Net;

namespace CashFlow.Exeption.ExceptionsBase;

public class CashFlowNotFoundException : CashFlowException
{
    public CashFlowNotFoundException(string message) : base(message) { }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [this.Message];
    }
}
