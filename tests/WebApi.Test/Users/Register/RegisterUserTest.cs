using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register;

public class RegisterUserTest(CashFlowWebApplicationFactory webApplicationFactory) : IClassFixture<CashFlowWebApplicationFactory>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();
    
    private const string Uri = "/api/user";
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var response = await _httpClient.PostAsJsonAsync(Uri, request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);
        
        json.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        json.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
        
        var response = await _httpClient.PostAsJsonAsync(Uri, request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var body = await response.Content.ReadAsStreamAsync();

        var json = await JsonDocument.ParseAsync(body);

        var errors = json.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NameIsRequired", new CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}