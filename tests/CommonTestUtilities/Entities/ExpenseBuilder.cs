using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities;

public static class ExpenseBuilder
{
    public static List<Expense> BuildList(User user, int count = 10)
    {
        return new Faker<Expense>()
            .RuleFor(expense => expense.Id, faker => faker.UniqueIndex)
            .RuleFor(expense => expense.Title, faker => faker.Commerce.ProductName())
            .RuleFor(expense => expense.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(expense => expense.Date, faker => faker.Date.Past())
            .RuleFor(expense => expense.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(expense => expense.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(expense => expense.UserId, _ => user.Id)
            .RuleFor(expense => expense.Tags, (faker, expense) => faker.Make(1, () => new Tag
            {
                Id = faker.UniqueIndex,
                Value = faker.PickRandom<TagType>(),
                ExpenseId = expense.Id
            }))
            .Generate(count);
    }
    
    public static Expense Build(User user)
    {
        return new Faker<Expense>()
            .RuleFor(expense => expense.Id, faker => faker.UniqueIndex)
            .RuleFor(expense => expense.Title, faker => faker.Commerce.ProductName())
            .RuleFor(expense => expense.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(expense => expense.Date, faker => faker.Date.Past())
            .RuleFor(expense => expense.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(expense => expense.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(expense => expense.UserId, _ => user.Id)
            .RuleFor(expense => expense.Tags, (faker, expense) => faker.Make(1, () => new Tag
            {
                Id = faker.UniqueIndex,
                Value = faker.PickRandom<TagType>(),
                ExpenseId = expense.Id
            }));
    }
}