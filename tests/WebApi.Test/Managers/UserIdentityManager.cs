using CashFlow.Domain.Entities;

namespace WebApi.Test.Managers;

public class UserIdentityManager(User user, string password, string token)
{
    public string GetName() => user.Name;
    public string GetEmail() => user.Email;
    public string GetPassword() => password;
    public string GetToken() => token;
}