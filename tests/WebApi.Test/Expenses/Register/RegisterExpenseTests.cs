using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Register;

public class RegisterExpenseTests : CashFlowClassFixture
{
    private const string Uri = "/api/expenses";
    
    private readonly string _token;

    public RegisterExpenseTests(CashFlowWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterExpenseJsonBuilder.Build();
        
        var response = await DoPostAsync(requestUri: Uri, request: request, token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        json.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Title_Name(string culture)
    {
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var response = await DoPostAsync(requestUri: Uri, request: request, token: _token, culture: culture);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        await using var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        var errors = json.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TitleIsRequired", new CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}