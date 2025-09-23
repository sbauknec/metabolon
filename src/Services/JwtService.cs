namespace metabolon.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using metabolon.Models;
using metabolon.Generic;
using Microsoft.IdentityModel.Tokens;

//JSonWebToken Service, für Session Authorisierung
//On Login wird in der Response ein Token zurückgeschickt, der in allen folgenden Requests im Header mitgeschickt wird um den User zu authorisieren

public class JwtService : IJwtService
{
    private readonly IConfiguration _settings;

    //Registrieren der Settings, JwtSettings inkludiert den SecretKey (Encryption), ExpiryMinutes, Issuer, Audience
    //================================================================//
    //!!FALLS SICH DIESE WERTE IRGENDWANN ÄNDERN SOLLTEN ODER MÜSSEN!!//
    //  Die Sektion, aus der diese Werte kommen heißt "JwtSettings"   //
    //  Und befindet sich in der Datei 'src/appsettings.json'         //
    //================================================================//

    public JwtService(IConfiguration config) => _settings = config;

    //Baue aus den Daten vom User einen Webtoken Authenticator, in dem die User ID und ein Erlöschungsdatum hinterlegt sind
    //Dieser Token wird via Claims erstellt, via SecurityTokenDescriptor beschrieben (mit Issuer(Beweis, dass der Token aus dem System kommt) und Subject(Beweis, dass der Token zu dem User gehört))
    //Er wird via Sha256 encryptet und mit dem TokenHandler in einen sendbaren AuthToken Typ umgewandelt und zurückgegeben
    public AuthToken GenerateToken(User u)
    {
        var key = Encoding.UTF8.GetBytes(_settings["JwtSettings:secretKey"]!);
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, u.Id.ToString())
        };

        var expires = DateTime.UtcNow.AddMinutes(int.Parse(_settings["JwtSettings:ExpiryMinutes"]!));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = _settings["JwtSettings:Issuer"],
            Audience = _settings["JwtSettings:Audience"],
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

//Datenklasse AuthToken zum Halten und Versenden
//Der Token ist der bereits encryptete Token aus der GenerateToken Funktion
//Expires ist ein Datum + Uhrzeit an dem der Token erlischt d.h. ungültig wird
public class AuthToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}
