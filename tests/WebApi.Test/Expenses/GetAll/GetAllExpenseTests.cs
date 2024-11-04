using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpenseTests : CashFlowClassFixture
{
    private const string Uri = "/api/expenses";
    
    private readonly string _token;

    public GetAllExpenseTests(CashFlowWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGetAsync(requestUri: Uri, token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        json.RootElement.GetProperty("data").EnumerateArray().Should().NotBeEmpty();
    }
}