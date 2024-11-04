namespace MailService.Exceptions.Mailer;

[Serializable]
public class MailFailedException : Exception
{
    public MailFailedException() { }
    public MailFailedException(string message) : base(message) { }
    public MailFailedException(string message, Exception inner) : base(message, inner) { }
}