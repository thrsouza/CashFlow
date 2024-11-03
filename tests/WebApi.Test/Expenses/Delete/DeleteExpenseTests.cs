using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete;

public class DeleteExpenseTests : CashFlowClassFixture
{
    private const string Uri = "/api/expenses";
    
    private readonly string _token;
    private readonly long _expenseId;

    public DeleteExpenseTests(CashFlowWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoDeleteAsync(requestUri: $"{Uri}/{_expenseId}", token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        response = await DoGetAsync(requestUri: $"{Uri}/{_expenseId}", token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var response = await DoDeleteAsync(requestUri: $"{Uri}/{1000}", token: _token, culture: culture);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await response.Content.ReadAsStreamAsync();
 
        var json = await JsonDocument.ParseAsync(body);

        var errors = json.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("DataNotFound", new CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}