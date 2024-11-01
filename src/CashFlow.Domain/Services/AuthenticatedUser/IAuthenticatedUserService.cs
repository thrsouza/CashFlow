using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Services.AuthenticatedUser;

public interface IAuthenticatedUserService
{
    Task<User> Get();
}