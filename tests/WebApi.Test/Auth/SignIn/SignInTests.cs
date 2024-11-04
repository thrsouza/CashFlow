using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Communication.Requests;
using FluentAssertions;

namespace WebApi.Test.Auth.SignIn;

public class SignInTests : CashFlowClassFixture
{
    private const string Uri = "/api/authorization";

    private readonly string _name;
    private readonly string _email;
    private readonly string _password;

    public SignInTests(CashFlowWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
        _name = webApplicationFactory.UserTeamMember.GetName();
        _email = webApplicationFactory.UserTeamMember.GetEmail();
        _password = webApplicationFactory.UserTeamMember.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestSignInJson { Email = _email, Password = _password };

        var response = await DoPostAsync(requestUri: Uri, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        json.RootElement.GetProperty("name").GetString().Should().Be(_name);
        json.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
}