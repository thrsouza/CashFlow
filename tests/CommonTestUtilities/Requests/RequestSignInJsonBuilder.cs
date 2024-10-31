using Bogus;
using CashFlow.Communication.Requests;
// ReSharper disable VariableHidesOuterVariable

namespace CommonTestUtilities.Requests;

public static class RequestSignInJsonBuilder
{
    public static RequestSignInJson Build()
    {
        var faker = new Faker();

        return new Faker<RequestSignInJson>()
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
