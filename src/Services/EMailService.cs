using Microsoft.Extensions.Options;
using MailKit;
using MimeKit;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MailKit.Security;

public class EMailService
{
    private readonly SmtpSettings _smtp;
    private readonly AppSettings _settings;

    //Registrieren der Settings, SmtpSettings inkludiert Host, Name, Interne Mailadresse usw - AppSettings hat die BasisUrl des Systems zum Bilden von Links
    public EMailService(IOptions<SmtpSettings> smtp, IOptions<AppSettings> settings)
    {
        _smtp = smtp.Value;
        _settings = settings.Value;
    }

    //Mail Sendungsprozess
    //Exakte Formweise, sowie Templateauswahl ist abhängig vom Zweck der Mail
    //Purpose = 0: Verifikationsmail, Dictionary enthält die keys name, token, toEmail
    public async void SendMailAsync(int purpose, Dictionary<string, string> values)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtp.FromName, _smtp.FromEmail));
        message.To.Add(InternetAddress.Parse(values["toEmail"]));

        string? html;
        switch (purpose)
        {
            case 0:
                var tokenLink = GenerateVerificationLink(values["token"]);
                html = ReadTemplate("TokenMail")
                    .Replace("{{name}}", values["name"])
                    .Replace("{{verificationLink}}", tokenLink);

                message.Subject = ":metabolon E-Mail Verifikation erforderlich";
                break;
            default:    //Keiner der aufgelisteten Werte trifft zu
                return;
        }

        var builder = new BodyBuilder
        {
            HtmlBody = html,
            TextBody = Regex.Replace(html, "<.*?>", string.Empty)
        };
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtp.Host, _smtp.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_smtp.Username, _smtp.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public string GenerateVerificationLink(string token) => $"{_settings.BaseUrl}/User/verify?token={token}";
    public string ReadTemplate(string name)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", $"{name}.html");
        return File.ReadAllText(path);
    }
}