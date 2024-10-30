namespace CashFlow.Communication.Requests;

public class RequestAuthorizationJson
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}