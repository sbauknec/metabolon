using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using metabolon.Models;
using Microsoft.IdentityModel.Tokens;

public class JwtService {
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config) => _config = config;

    public string GenerateToken(User u)
    {
        var key = Encoding.UTF8.GetBytes(_config["JwtSettings:secretKey"]!);
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, u.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["JwtSettings:ExpiryMinutes"]!)),
            Issuer = _config["JwtSettings:Issuer"],
            Audience = _config["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}