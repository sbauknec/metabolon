using System.ComponentModel;
using Microsoft.Extensions.Options;

public class EMailService
{
    private readonly AppSettings _settings;
    public EMailService(IOptions<AppSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateVerificationLink(string token) => $"{_settings.BaseUrl}/User/verify?token={token}"; 
}