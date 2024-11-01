using CashFlow.Domain.Entities;
using CashFlow.Domain.Security;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CashFlowWebApplicationFactory : WebApplicationFactory<Program>
{
    private User _user;
    private string _password;

    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    
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
            });
    }

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncryptor passwordEncryptor)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        
        _user.Password = passwordEncryptor.Encrypt(_user.Password);
        
        dbContext.Users.Add(_user);

        dbContext.SaveChanges();
    }
}