using CashFlow.Domain.Security;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security;

internal class PasswordEncryptor : IPasswordEncryptor
{
    public string Encrypt(string password)
    {
        return BC.HashPassword(password);
    }

    public bool Verify(string password, string passwordHash)
    {
        return BC.Verify(password, passwordHash);
    }
}