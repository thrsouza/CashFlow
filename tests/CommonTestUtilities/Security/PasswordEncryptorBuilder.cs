using CashFlow.Domain.Security;
using Moq;

namespace CommonTestUtilities.Security;

public class PasswordEncryptorBuilder
{
    private readonly Mock<IPasswordEncryptor> _mock = new();

    public PasswordEncryptorBuilder()
    {
        _mock.Setup(x => x.Encrypt(It.IsAny<string>()))
            .Returns("$2a$11$a8FLvyin4/um4NoF7vXVrOZ6gODXIR3F0HiSGrCYkOH9Kwgki0WR.");
    }

    public PasswordEncryptorBuilder Verify(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password))
        {
            _mock.Setup(x => x.Verify(password, It.IsAny<string>())).Returns(true);
        }

        return this;
    }

    public IPasswordEncryptor Build() => _mock.Object;
}