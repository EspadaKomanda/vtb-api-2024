namespace MailService.Exceptions.Mail;

[Serializable]
public class MailTimeoutException : Exception
{
    public MailTimeoutException() { }
    public MailTimeoutException(string message) : base(message) { }
    public MailTimeoutException(string message, Exception inner) : base(message, inner) { }
}