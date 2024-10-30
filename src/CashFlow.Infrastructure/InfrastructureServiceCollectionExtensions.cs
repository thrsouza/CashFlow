using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.DataAccess.Repositories.Expenses;
using CashFlow.Infrastructure.DataAccess.Repositories.Users;
using CashFlow.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
        AddAccessTokenGenerator(services, configuration);
        
        services.AddScoped<IPasswordEncryptor, PasswordEncryptor>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<CashFlowDbContext>(optionsBuilder => 
        {
            optionsBuilder.UseSqlServer(connectionString);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpensesUpdateOnlyRepository, ExpensesRepository>();

        services.AddScoped<IUsersReadOnlyRepository, UsersRepository>();
        services.AddScoped<IUsersWriteOnlyRepository, UsersRepository>();
    }
    
    private static void AddAccessTokenGenerator(IServiceCollection services, IConfiguration configuration)
    {
        var expirationInMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresInMinutes");
        var secret = configuration.GetValue<string>("Settings:Jwt:Secret");
        
        services.AddScoped<IAccessTokenGenerator, AccessTokenGenerator>(_ => new AccessTokenGenerator(expirationInMinutes, secret!));
    }
}