namespace CashFlow.Domain.Security.Cryptography;

public interface IPasswordEncryptor
{
    string Encrypt(string password);
    bool Verify(string password, string passwordHash);
}