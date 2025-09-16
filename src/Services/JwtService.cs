namespace metabolon.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using metabolon.Models;
using metabolon.Generic;
using Microsoft.IdentityModel.Tokens;

public class JwtService : IJwtService {
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config) => _config = config;

    public AuthToken GenerateToken(User u)
    {
        var key = Encoding.UTF8.GetBytes(_config["JwtSettings:secretKey"]!);
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, u.Id.ToString())
        };

        var expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["JwtSettings:ExpiryMinutes"]!));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = _config["JwtSettings:Issuer"],
            Audience = _config["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return new AuthToken
        {
            Token = tokenHandler.WriteToken(token),
            Expires = expires
        };
    }
}

public class AuthToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}
