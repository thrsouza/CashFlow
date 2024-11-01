using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Communication.Requests;
using FluentAssertions;

namespace WebApi.Test.Auth.SignIn;

public class SignInTests(CashFlowWebApplicationFactory webApplicationFactory)
    : IClassFixture<CashFlowWebApplicationFactory>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();
    
    private readonly string _name = webApplicationFactory.GetName();
    private readonly string _email = webApplicationFactory.GetEmail();
    private readonly string _password = webApplicationFactory.GetPassword();
    
    private const string Uri = "/api/authorization";

    [Fact]
    public async Task Success()
    {
        var request = new RequestSignInJson { Email = _email, Password = _password };

        var response = await _httpClient.PostAsJsonAsync(Uri, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        json.RootElement.GetProperty("name").GetString().Should().Be(_name);
        json.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
}