using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UsersReadOnlyRepositoryBuilder
{
    private readonly Mock<IUsersReadOnlyRepository> _repository = new();

    public UsersReadOnlyRepositoryBuilder ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(x => x.ExistActiveUserWithEmail(email)).ReturnsAsync(true);

        return this;
    }
    
    public UsersReadOnlyRepositoryBuilder GetByEmail(User user)
    {
        _repository.Setup(x => x.GetByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }
    
    public IUsersReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}