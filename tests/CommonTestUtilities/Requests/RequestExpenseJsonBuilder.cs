using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
// ReSharper disable VariableHidesOuterVariable

namespace CommonTestUtilities.Requests;

public static class RequestExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
        return new Faker<RequestExpenseJson>()
            .RuleFor(expense => expense.Title, faker => faker.Commerce.ProductName())
            .RuleFor(expense => expense.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(expense => expense.Date, faker => faker.Date.Past())
            .RuleFor(expense => expense.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(expense => expense.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(expense => expense.Tags, faker => faker.Make(1, faker.PickRandom<TagType>));
    }
}
