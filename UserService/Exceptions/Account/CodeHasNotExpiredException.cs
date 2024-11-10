namespace UserService.Exceptions.Account;

[Serializable]
public class CodeHasNotExpiredException : Exception
{
    public CodeHasNotExpiredException() { }
    public CodeHasNotExpiredException(string message) : base(message) { }
    public CodeHasNotExpiredException(string message, Exception inner) : base(message, inner) { }
}