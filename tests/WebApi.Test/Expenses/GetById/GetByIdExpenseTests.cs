using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.GetById;

public class GetByIdExpenseTests(CashFlowWebApplicationFactory webApplicationFactory) : CashFlowClassFixture(webApplicationFactory)
{
    private readonly CashFlowWebApplicationFactory _webApplicationFactory = webApplicationFactory;
    private const string Uri = "/api/expenses";

    [Fact]
    public async Task Success()
    {
        var expenseId = _webApplicationFactory.GetExpenseId();
        
        var response = await DoGetAsync(requestUri: $"{Uri}/{expenseId}", token: _webApplicationFactory.GetToken());

        var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        json.RootElement.GetProperty("id").GetInt64().Should().Be(expenseId);
        json.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        json.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
        json.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
        json.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);
        
        var paymentType = json.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var response = await DoGetAsync(requestUri: $"{Uri}/{1000}", token: _webApplicationFactory.GetToken(), culture: culture);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await response.Content.ReadAsStreamAsync();
 
        var json = await JsonDocument.ParseAsync(body);

        var errors = json.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("DataNotFound", new CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}