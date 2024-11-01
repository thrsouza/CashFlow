using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.AccessToken;
using Moq;

namespace CommonTestUtilities.Security;

public static class AccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();
        
        mock.Setup(x => x.Generate(It.IsAny<User>()))
            .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");
        
        return mock.Object;
    }
}