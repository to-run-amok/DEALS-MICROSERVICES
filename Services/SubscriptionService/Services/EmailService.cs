using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(
        string to,
        string subject,
        string body)
    {
        using var client = new SmtpClient(
            _configuration["SmtpSettings:Server"],
            int.Parse(
                _configuration["SmtpSettings:Port"]!));

        client.Credentials =
            new NetworkCredential(
                _configuration["SmtpSettings:Username"],
                _configuration["SmtpSettings:Password"]);

        client.EnableSsl = true;

        Console.WriteLine(
            $"SMTP Server: {_configuration["SmtpSettings:Server"]}");

        Console.WriteLine(
            $"SMTP User: {_configuration["SmtpSettings:Username"]}");

        Console.WriteLine(
            $"To: {to}");

        var mail = new MailMessage(
            _configuration["SmtpSettings:SenderEmail"]!,
            to,
            subject,
            body);

        await client.SendMailAsync(mail);
    }
}