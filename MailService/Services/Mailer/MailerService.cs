using System.Net.Mail;
using MailService.Models.Mail.Requests;
using MailService.Models.Mail.Responses;
using MailService.Exceptions.Mailer;

namespace MailService.Services.Mailer;

public class MailerService(IConfiguration configuration, ILogger<MailerService> logger) : IMailerService
{
    private readonly IConfiguration _conf = configuration;
    private readonly ILogger<MailerService> _logger = logger;
    public SendMailResponse SendMail(SendMailRequest request)
    {
        if (request.IsDummy)
        {
            _logger.LogDebug("Pretending to successfully send an email: {request}", request);
            return new SendMailResponse { IsSuccess = true };
        }

        try
        {
            SmtpClient mySmtpClient = new(
                _conf["SmtpClient:Host"] ?? throw new ConfigurationMissingException("SmtpClient:Host"),
                int.Parse(_conf["SmtpClient:Port"] ?? throw new ConfigurationMissingException("SmtpClient:Port"))
            );

            mySmtpClient.UseDefaultCredentials = false;
            System.Net.NetworkCredential basicAuthenticationInfo = new(
                _conf["SmtpClient:Username"] ?? throw new ConfigurationMissingException("SmtpClient:Username"),
                _conf["SmtpClient:Password"] ?? throw new ConfigurationMissingException("SmtpClient:Password")
            );
            mySmtpClient.Credentials = basicAuthenticationInfo;

            MailAddress from = new(
                _conf["SmtpClient:Username"] ?? throw new ConfigurationMissingException("SmtpClient:Username"), 
                _conf["SmtpClient:FromName"] ?? throw new ConfigurationMissingException("SmtpClient:FromName")
            );
            MailAddress to = new(request.ToAddress);
            MailMessage myMail = new(from, to);

            myMail.Subject = request.Subject;
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            myMail.Body = request.Body;
            myMail.BodyEncoding = System.Text.Encoding.UTF8;

            myMail.IsBodyHtml = request.IsHtml;

            try
            {
                mySmtpClient.Send(myMail);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to send email: {request}", request);
                throw new MailFailedException();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Email was not sent: {request}", request);
            throw;
        }
        return  new SendMailResponse { IsSuccess = true };
    }
}