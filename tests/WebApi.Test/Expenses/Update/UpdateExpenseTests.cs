using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Update;

public class UpdateExpenseTests : CashFlowClassFixture
{
    private const string Uri = "/api/expenses";
    
    private readonly string _token;
    private readonly long _expenseId;

    public UpdateExpenseTests(CashFlowWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _expenseId = webApplicationFactory.ExpenseTeamMember.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();
        
        var response = await DoPutAsync(requestUri: $"{Uri}/{_expenseId}", request: request, token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Title_Name(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var response = await DoPutAsync(requestUri: $"{Uri}/{_expenseId}", request: request, token: _token, culture: culture);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        await using var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        var errors = json.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TitleIsRequired", new CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        
        var response = await DoPutAsync(requestUri: $"{Uri}/{1000}", request: request, token: _token, culture: culture);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var body = await response.Content.ReadAsStreamAsync();
 
        var json = await JsonDocument.ParseAsync(body);

        var errors = json.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("DataNotFound", new CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}