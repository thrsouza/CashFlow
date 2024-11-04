using System.Net;
using CashFlow.Communication.Requests;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace WebApi.Test.Users.Delete;

public class DeleteUserAccountTests : CashFlowClassFixture
{
    private const string Uri = "/api/users";
    
    private readonly string _token;
    
    public DeleteUserAccountTests(CashFlowWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }
    
    [Fact]
    public async Task Success()
    {
        var response = await DoDeleteAsync(requestUri: Uri, token: _token);
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}