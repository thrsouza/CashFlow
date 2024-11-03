using System.Security.AccessControl;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.AccessToken;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.Security.AccessToken;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Managers;

namespace WebApi.Test;

public class CashFlowWebApplicationFactory : WebApplicationFactory<Program>
{
    public UserIdentityManager UserAdministrator { get; private set; } = default!;
    public UserIdentityManager UserTeamMember { get; private set; } = default!;
    public ExpenseIdentityManager ExpenseAdministrator { get; private set; } = default!;
    public ExpenseIdentityManager ExpenseTeamMember { get; private set; } = default!;
    
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
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncryptor, accessTokenGenerator);
            });
    }

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncryptor passwordEncryptor, IAccessTokenGenerator accessTokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncryptor, accessTokenGenerator);
        var expenseTeamMember = AddExpense(dbContext, userTeamMember);
        ExpenseTeamMember = new ExpenseIdentityManager(expense: expenseTeamMember);
        
        var userAdministrator = AddUserAdministrator(dbContext, passwordEncryptor, accessTokenGenerator);
        var expenseAdministrator = AddExpense(dbContext, userAdministrator);
        ExpenseAdministrator = new ExpenseIdentityManager(expense: expenseAdministrator);
        
        dbContext.SaveChanges();
    }

    private User AddUserAdministrator(CashFlowDbContext dbContext, IPasswordEncryptor passwordEncryptor, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.Admin);
        var password = user.Password;
        
        user.Password = passwordEncryptor.Encrypt(user.Password);
        
        dbContext.Users.Add(user); 
        
        var token = accessTokenGenerator.Generate(user);
        
        UserAdministrator = new UserIdentityManager(user: user, password: password, token: token);

        return user;
    }
    
    private User AddUserTeamMember(CashFlowDbContext dbContext, IPasswordEncryptor passwordEncryptor, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        var password = user.Password;
        
        user.Password = passwordEncryptor.Encrypt(user.Password);
        
        dbContext.Users.Add(user); 
        
        var token = accessTokenGenerator.Generate(user);
        
        UserTeamMember = new UserIdentityManager(user: user, password: password, token: token);

        return user;
    }
    
    private Expense AddExpense(CashFlowDbContext dbContext, User user)
    {
        var expense = ExpenseBuilder.Build(user);
        
        dbContext.Expenses.Add(expense);
        
        return expense;
    }
}