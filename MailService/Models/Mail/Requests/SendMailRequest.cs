using System.ComponentModel.DataAnnotations;

namespace MailService.Models.Mail.Requests;

public class SendMailRequest
{
    [Required]
    public string ToAddress { get; set; } = null!;
    public string? Subject { get; set; }
    public string Body { get; set; } = null!;
    public bool IsHtml { get; set; } = false;
    public bool IsDummy { get; set; } = false;

}