namespace metabolon.Services;

using Microsoft.Extensions.Options;
using MailKit;
using MimeKit;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MailKit.Security;
using metabolon.Generic;

public class EMailService : IEmailService
{
    private readonly SmtpSettings _smtp;
    private readonly AppSettings _settings;

    //Registrieren der Settings, SmtpSettings inkludiert Host, Name, Interne Mailadresse usw - AppSettings hat die BasisUrl des Systems zum Bilden von Links
    //================================================================//
    //!!FALLS SICH DIESE WERTE IRGENDWANN ÄNDERN SOLLTEN ODER MÜSSEN!!//
    //  Die Sektion, aus der diese Werte kommen heißt "SmtpSettings"  //
    //  Und befindet sich in der Datei 'src/appsettings.json'         //
    //================================================================//
    public EMailService(IOptions<SmtpSettings> smtp, IOptions<AppSettings> settings)
    {
        _smtp = smtp.Value;
        _settings = settings.Value;
    }

    //Mail Sendungsprozess
    //Exakte Formweise, sowie Templateauswahl ist abhängig vom Zweck der Mail
    //Purpose = 0: Verifikationsmail, Dictionary enthält die keys name, token, toEmail
    //Purpose = 1: Dokumentmail, Dictionary enthält die keys name, requestingName, documentId, toEmail
    public async Task SendMailAsync(int purpose, Dictionary<string, string> values)
    {
        //Bau eine MimeMessage und schreibe zunächst nur Adressen von Sender und Empfänger
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtp.FromName, _smtp.FromEmail));
        message.To.Add(InternetAddress.Parse(values["toEmail"]));

        //Switch zum Grippen des richtigen Templates, auf basis von 'purpose' d.h. jeder Wert den dieser purpose annehmen kann bedeutet dass die Mail zu einem anderen Zweck geschickt wird und entsprechend ein anderes Template braucht
        //Das Dictionary kommt hier primär zum Einsatz um Placeholder im Template mit korrekten Daten zu ersetzen
        //Zudem wird das Subject oder die Betreffzeile gefüllt
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
            case 1:
                var docLink = Path.Combine(_settings.BaseUrl!, values["documentId"]);
                html = ReadTemplate("DocumentRequestMail")
                        .Replace("{{name}}", values["name"])
                        .Replace("{{requestingName}}", values["requestingName"])
                        .Replace("{{link}}", docLink);
                break;
            default:    //Keiner der aufgelisteten Werte trifft zu
                return;
        }

        //BodyBuilder baut den Hauptteil der Mail, den Text
        //Weil manche (alte) Mailverteiler wie z.B. Outlook manche Mails ausfiltert an Kriterien die mir nicht bekannt sind kann es sein, dass das HTML vom Template abgelehnt wird
        //Fallback ist den Body der Mail auch als Plain-Text zu hinterlegen, was in diesem Fall einfach ist, wir ziehen sämtliche Tags raus die HTML sein könnten mit einer simplen REGEX Operation
        var builder = new BodyBuilder
        {
            HtmlBody = html,
            TextBody = Regex.Replace(html, "<.*?>", string.Empty)
        };
        message.Body = builder.ToMessageBody();

        //Aufbauen einer Verbindung zum SMTP Server via SmtpClient()
        //Abschicken der Mail und sorgfältige Terminierung der SMTP Verbindung
        //Alle genutzten Daten kommen aus SmtpSettings
        using var client = new SmtpClient();
        await client.ConnectAsync(_smtp.Host, _smtp.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_smtp.Username, _smtp.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    //Generiert Verifikationslink mit der Basis-URL (sowas wie https://metabolon.de oder ähnliches) aus den Appsettings und dem generierten Token, der aus der User Anlegung mitgegeben wird
    public string GenerateVerificationLink(string token) => $"{_settings.BaseUrl}/User/verify?token={token}";

    //Liest ein Template aus, anhand von 'name', indem es den Filepath baut relativ zu dieser Datei und das Template in reinem Text als String zurückgibt
    //Die Templates liegen im Ordner src/Services/EmailTemplates
    public string ReadTemplate(string name)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", $"{name}.html");
        return File.ReadAllText(path);
    }
}