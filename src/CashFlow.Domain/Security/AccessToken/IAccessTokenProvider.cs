namespace CashFlow.Domain.Security.AccessToken;

public interface IAccessTokenProvider
{
    string TokenOnRequest();
}