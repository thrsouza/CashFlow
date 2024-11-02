using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.AccessToken;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.Security.AccessToken;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CashFlowWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly User _user;
    private readonly Expense _expense;
    
    private readonly string _password;
    private string _token = null!;

    public CashFlowWebApplicationFactory()
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        
        _expense = ExpenseBuilder.Build(_user);
    }
    
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetToken() => _token;
    public long GetExpenseId() => _expense.Id;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                
                services.AddDbContext<CashFlowDbContext>(optionsBuilder => 
                {
                    optionsBuilder.UseInMemoryDatabase("InMemoryDbForTesting");
                    optionsBuilder.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncryptor = scope.ServiceProvider.GetRequiredService<IPasswordEncryptor>();
                
                StartDatabase(dbContext, passwordEncryptor);
                
                // Generate token for integration tests
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();
                _token = accessTokenGenerator.Generate(_user);
            });
    }

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncryptor passwordEncryptor)
    {
        AddUsers(dbContext, passwordEncryptor);
        AddExpenses(dbContext);
    }

    private void AddUsers(CashFlowDbContext dbContext, IPasswordEncryptor passwordEncryptor)
    {
        _user.Password = passwordEncryptor.Encrypt(_user.Password);
        dbContext.Users.Add(_user);
        dbContext.SaveChanges();
    }
    
    private void AddExpenses(CashFlowDbContext context)
    {
        context.Expenses.Add(_expense);
        context.SaveChanges();
    }
}