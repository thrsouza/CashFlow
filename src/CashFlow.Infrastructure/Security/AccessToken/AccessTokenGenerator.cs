using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.AccessToken;
using Microsoft.IdentityModel.Tokens;

namespace CashFlow.Infrastructure.Security.AccessToken;

public class AccessTokenGenerator(uint expirationInMinutes, string secret) 
    : IAccessTokenGenerator
{
    public string Generate(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, user.UserIdentifier.ToString()),
            }),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}