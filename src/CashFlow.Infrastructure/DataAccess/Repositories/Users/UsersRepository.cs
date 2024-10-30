using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories.Users;

internal class UsersRepository(CashFlowDbContext dbContext) : IUsersReadOnlyRepository, IUsersWriteOnlyRepository
{
    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        var result = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email.Equals(email));
        
        return result;
    }

    public async Task Add(User user)
    {
        await dbContext.Users.AddAsync(user);
    }
}