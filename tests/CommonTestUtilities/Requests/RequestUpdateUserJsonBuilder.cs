using Bogus;
using CashFlow.Communication.Requests;
// ReSharper disable VariableHidesOuterVariable

namespace CommonTestUtilities.Requests;

public static class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        var faker = new Faker();

        return new Faker<RequestUpdateUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
    }
}