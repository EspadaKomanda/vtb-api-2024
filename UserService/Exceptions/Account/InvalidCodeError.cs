namespace UserService.Exceptions.Account;

[Serializable]
public class InvalidCodeException : Exception
{
    public InvalidCodeException() { }
    public InvalidCodeException(string message) : base(message) { }
    public InvalidCodeException(string message, Exception inner) : base(message, inner) { }
}