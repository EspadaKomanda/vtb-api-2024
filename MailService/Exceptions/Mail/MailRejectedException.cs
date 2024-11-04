namespace MailService.Exceptions.Mail;

[Serializable]
public class MailRejectedException : Exception
{
    public MailRejectedException() { }
    public MailRejectedException(string message) : base(message) { }
    public MailRejectedException(string message, Exception inner) : base(message, inner) { }
}