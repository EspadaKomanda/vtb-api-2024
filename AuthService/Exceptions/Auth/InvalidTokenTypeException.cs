namespace AuthService.Exceptions.Auth;

[Serializable]
public class InvalidTokenTypeException : Exception
{
    public InvalidTokenTypeException() { }
    public InvalidTokenTypeException(string message) : base(message) { }
    public InvalidTokenTypeException(string message, Exception inner) : base(message, inner) { }
}