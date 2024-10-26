namespace CashFlow.Communication.Responses;

public class ResponseErrorJson
{
    public List<string> ErrorMessages { get; }

    public ResponseErrorJson(List<string> errorMessages)
    {
        this.ErrorMessages = errorMessages;
    }
}
