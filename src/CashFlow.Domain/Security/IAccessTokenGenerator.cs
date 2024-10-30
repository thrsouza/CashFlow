using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}