using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PetsProjects.Models;

namespace PetsProjects;

public class JwtProvider(IOptions<JwtOptions> options) //буду хранить небезопасным способом в appsettings
    : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateToken(UserModel user)
    {
        Claim[] claims =
            [
                new ("userId", user.Id.ToString()),
                new ("Admin","Write")
            ];
        
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256); 
        
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));
        
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        
        return tokenValue;
    }
}