using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.AccessToken;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}