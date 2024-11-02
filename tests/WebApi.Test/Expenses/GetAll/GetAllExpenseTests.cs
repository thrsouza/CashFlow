using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpenseTests(CashFlowWebApplicationFactory webApplicationFactory) : CashFlowClassFixture(webApplicationFactory)
{
    private readonly CashFlowWebApplicationFactory _webApplicationFactory = webApplicationFactory;
    private const string Uri = "/api/expenses";

    [Fact]
    public async Task Success()
    {
        var response = await DoGetAsync(requestUri: Uri, token: _webApplicationFactory.GetToken());

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        json.RootElement.GetProperty("data").EnumerateArray().Should().NotBeEmpty();
    }
}