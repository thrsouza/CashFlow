using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.AuthenticatedUser;
using Moq;

namespace CommonTestUtilities.Services;

public class AuthenticatedUserServiceBuilder
{
    public static IAuthenticatedUserService Build(User user)
    {
        var mock = new Mock<IAuthenticatedUserService>();

        mock.Setup(service => service.Get()).ReturnsAsync(user);
        
        return mock.Object;
    }
}