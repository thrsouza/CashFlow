using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.AccessToken;
using CashFlow.Domain.Services.AuthenticatedUser;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Services.AuthenticatedUser;

public class AuthenticatedUserService(CashFlowDbContext dbContext, IAccessTokenProvider accessTokenProvider) : IAuthenticatedUserService
{
    public async Task<User> Get()
    {
        var token = accessTokenProvider.TokenOnRequest();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var tokenContent = tokenHandler.ReadJwtToken(token);
        
        var userIdentifier = tokenContent.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;
        
        return await dbContext.Users.AsNoTracking()
            .FirstAsync(user => user.UserIdentifier == Guid.Parse(userIdentifier));
    }
}