using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories.Users;

internal class UsersRepository(CashFlowDbContext dbContext) : IUsersReadOnlyRepository, IUsersWriteOnlyRepository, IUsersUpdateOnlyRepository
{
    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        var result = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email.Equals(email));
        
        return result;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task Add(User user)
    {
        await dbContext.Users.AddAsync(user);
    }

    public async Task<User> GetById(long id)
    {
        return await dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public void Update(User user)
    {
        dbContext.Users.Update(user);
    }
    
    public async Task Delete(long id)
    {
        var user = await dbContext.Users.FirstAsync(user => user.Id == id);
        
        dbContext.Users.Remove(user);
    }
}