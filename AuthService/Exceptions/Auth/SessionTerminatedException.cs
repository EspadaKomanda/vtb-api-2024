namespace AuthService.Exceptions.Auth;

[Serializable]
public class SessionTerminatedException : Exception
{
    public SessionTerminatedException() { }
    public SessionTerminatedException(string message) : base(message) { }
    public SessionTerminatedException(string message, Exception inner) : base(message, inner) { }
}