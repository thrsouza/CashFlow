using Bogus;
using CashFlow.Communication.Requests;
// ReSharper disable VariableHidesOuterVariable

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build()
    {
        var faker = new Faker();

        return new Faker<RequestRegisterUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}