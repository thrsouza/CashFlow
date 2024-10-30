namespace CashFlow.Domain.Security;

public interface IPasswordEncryptor
{
    string Encrypt(string password);
    bool Verify(string password, string passwordHash);
}