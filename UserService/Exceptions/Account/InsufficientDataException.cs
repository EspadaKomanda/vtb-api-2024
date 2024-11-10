namespace UserService.Exceptions.Account;

[Serializable]
public class InsufficientDataException : Exception
{
    public InsufficientDataException() { }
    public InsufficientDataException(string message) : base(message) { }
    public InsufficientDataException(string message, Exception inner) : base(message, inner) { }
}