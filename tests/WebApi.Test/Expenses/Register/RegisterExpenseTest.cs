using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Register;

public class RegisterExpenseTest(CashFlowWebApplicationFactory webApplicationFactory) : CashFlowClassFixture(webApplicationFactory)
{
    private readonly CashFlowWebApplicationFactory _webApplicationFactory = webApplicationFactory;
    private const string Uri = "/api/expenses";

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterExpenseJsonBuilder.Build();
        
        var response = await DoPostAsync(requestUri: Uri, request: request, token: _webApplicationFactory.GetToken());

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        json.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Title_Name(string culture)
    {
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var response = await DoPostAsync(requestUri: Uri, request: request, token: _webApplicationFactory.GetToken(), culture: culture);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        var errors = json.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TitleIsRequired", new CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}