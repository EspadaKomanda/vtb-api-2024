[System.Serializable]
public class InvalidPasswordException : System.Exception
{
    public InvalidPasswordException() { }
    public InvalidPasswordException(string message) : base(message) { }
    public InvalidPasswordException(string message, System.Exception inner) : base(message, inner) { }
}