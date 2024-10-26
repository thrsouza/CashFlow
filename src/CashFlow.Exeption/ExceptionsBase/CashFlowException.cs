namespace CashFlow.Exeption.ExceptionsBase;

public abstract class CashFlowException : SystemException
{
    public abstract int StatusCode { get; }

    protected CashFlowException(string message) : base(message) { }

    public abstract List<string> GetErrors();
}
