namespace metabolon.Generic;

using metabolon.Services;

public interface IEmailService
{
    Task SendMailAsync(int purpose, Dictionary<string, string> values);
}