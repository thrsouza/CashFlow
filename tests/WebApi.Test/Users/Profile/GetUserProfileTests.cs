using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebApi.Test.Users.Profile;

public class GetUserProfileTests : CashFlowClassFixture
{
    private const string Uri = "api/users"; 
    
    private readonly string _token;
    private readonly string _email;
    private readonly string _name;
    
    public GetUserProfileTests(CashFlowWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _email = webApplicationFactory.UserTeamMember.GetEmail();
        _name = webApplicationFactory.UserTeamMember.GetName();
    }
    
    [Fact]
    public async Task Success()
    {
        var response = await DoGetAsync(requestUri: Uri, token: _token);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        await using var body = await response.Content.ReadAsStreamAsync();
        
        var json = await JsonDocument.ParseAsync(body);
        
        json.RootElement.GetProperty("name").GetString().Should().Be(_name);
        json.RootElement.GetProperty("email").GetString().Should().Be(_email);
    }
    
    [Fact]
    public async Task Unauthorized()
    {
        var response = await DoGetAsync(requestUri: Uri);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}