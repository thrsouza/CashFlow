using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.ChangePassword;

public class ChangePasswordTests: CashFlowClassFixture
{
    private const string Uri = "api/users/change-password"; 
    
    private readonly string _token;
    private readonly string _password;
    private readonly string _email;
    
    public ChangePasswordTests(CashFlowWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _password = webApplicationFactory.UserTeamMember.GetPassword();
        _email = webApplicationFactory.UserTeamMember.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;
        
        var response = await DoPutAsync(requestUri: Uri, request: request, token: _token);
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var loginRequest = new RequestSignInJson { Email = _email, Password = _password };
        var loginResponse = await DoPostAsync(requestUri: "api/authorization", request: loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        loginRequest = new RequestSignInJson { Email = _email, Password = request.NewPassword };
        loginResponse = await DoPostAsync(requestUri: "api/authorization", request: loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_CurrentPassword_Different(string culture)
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        
        var response = await DoPutAsync(requestUri: Uri, request: request, token: _token, culture: culture);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        await using var body = await response.Content.ReadAsStreamAsync();
        
        var json = await JsonDocument.ParseAsync(body);

        var errors = json.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PasswordIncorrect", new CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}