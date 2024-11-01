using CashFlow.Domain.Security.AccessToken;

namespace CashFlow.Api.Providers;

public class AccessTokenProvider(IHttpContextAccessor httpContextAccessor) : IAccessTokenProvider
{
    public string TokenOnRequest()
    {
        var authorization = httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        
        return authorization["Bearer ".Length..].Trim();
    }
}