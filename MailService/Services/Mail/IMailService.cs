using MailService.Models.Mail.Requests;
using MailService.Models.Mail.Responses;

namespace MailService.Services.Mail;

public interface IMailService
{
    Task<SendMailResponse> SendMailAsync(SendMailRequest request);
}