using System.Globalization;

namespace CashFlow.Api.Middlewares;

public class CultureMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        var requestedLanguage = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        var cultureInfo = new CultureInfo("en");

        if (!string.IsNullOrWhiteSpace(requestedLanguage) 
            && supportedLanguages.Exists(language => language.Name == requestedLanguage))
        {
            cultureInfo = new CultureInfo(requestedLanguage);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }

        await next(context);
    }
}
