using System.Net;
using System.Net.Mime;
using FluentAssertions;

namespace WebApi.Test.Expenses.Reports;

public class GenerateExpenseReportTests : CashFlowClassFixture
{
    private const string RequestUri = "api/reports";

    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly DateTime _expenseDate;

    public GenerateExpenseReportTests(CashFlowWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.UserAdministrator.GetToken();
        _teamMemberToken = webApplicationFactory.UserTeamMember.GetToken();
        _expenseDate = webApplicationFactory.ExpenseAdministrator.GetDate();
    }

    [Fact]
    public async Task Success_Excel()
    {
        var response = await DoGetAsync(requestUri: $"{RequestUri}/excel?month={_expenseDate:Y}", token: _adminToken);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
       
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }   
    
    [Fact]
    public async Task Success_Pdf()
    {
        var response = await DoGetAsync(requestUri: $"{RequestUri}/pdf?month={_expenseDate:Y}", token: _adminToken);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
       
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    } 
    
    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGetAsync(requestUri: $"{RequestUri}/excel?month={_expenseDate:Y}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGetAsync(requestUri: $"{RequestUri}/pdf?month={_expenseDate:Y}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}