using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUsersUpdateOnlyRepository> _repository = new();

    public UserUpdateOnlyRepositoryBuilder GetById(User user)
    {
        _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }

    public IUsersUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }
}