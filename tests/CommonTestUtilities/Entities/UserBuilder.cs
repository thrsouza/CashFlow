using Bogus;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Security;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
    public static User Build()
    {
        var passwordEncryptor = new PasswordEncryptorBuilder().Build();
        
        var user = new Faker<User>()
            .RuleFor(user => user.Id, faker => faker.UniqueIndex)
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => passwordEncryptor.Encrypt(faker.Internet.Password()))
            .RuleFor(user => user.UserIdentifier, faker => faker.Random.Guid());
        
        return user;
    }
}