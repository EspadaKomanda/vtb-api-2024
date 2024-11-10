using MailService.Models.Mail.Requests;
using MailService.Models.Mail.Responses;

namespace MailService.Services.Mailer;

public interface IMailerService
{
    SendMailResponse SendMail(SendMailRequest request);
}