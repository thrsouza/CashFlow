using Microsoft.Extensions.Configuration;

namespace CashFlow.Infrastructure;

public static class InfrastructureConfigurationExtensions
{
    public static bool IsTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTesting");
    } 
}