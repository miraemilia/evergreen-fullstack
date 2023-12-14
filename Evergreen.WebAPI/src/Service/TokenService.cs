using Evergreen.Core.src.Entity;
using Evergreen.Service.src.Abstraction;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Evergreen.WebAPI.src.Service;

public class TokenService : ITokenService
{
    private IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }
    
    public string GenerateToken(User user)
    {
        var issuer = _config.GetSection("Jwt:Issuer").Value;
        var claims = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var audience = _config.GetSection("Jwt:Audience").Value;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));
        var signingKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.Now.AddDays(2),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingKey
        };
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }
}