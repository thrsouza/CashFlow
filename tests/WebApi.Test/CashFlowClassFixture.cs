using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class CashFlowClassFixture(CashFlowWebApplicationFactory webApplicationFactory)
    : IClassFixture<CashFlowWebApplicationFactory>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();

    protected async Task<HttpResponseMessage> DoPostAsync(string requestUri, object request, string token = "", string culture = "en")
    {
        SetAuthorizationToken(token);
        SetCulture(culture);
        
        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }
    
    protected async Task<HttpResponseMessage> DoGetAsync(string requestUri, string token = "", string culture = "en")
    {
        SetAuthorizationToken(token);
        SetCulture(culture);
        
        return await _httpClient.GetAsync(requestUri);
    }
    
    protected async Task<HttpResponseMessage> DoDeleteAsync(string requestUri, string token = "", string culture = "en")
    {
        SetAuthorizationToken(token);
        SetCulture(culture);
        
        return await _httpClient.DeleteAsync(requestUri);
    }
    

    private void SetAuthorizationToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return;
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    private void SetCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}