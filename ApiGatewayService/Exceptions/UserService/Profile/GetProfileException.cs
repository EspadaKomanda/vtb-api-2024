namespace ApiGatewayService.Exceptions.UserService.Profile;

[System.Serializable]
public class GetProfileException : System.Exception
{
    public GetProfileException() { }
    public GetProfileException(string message) : base(message) { }
    public GetProfileException(string message, System.Exception inner) : base(message, inner) { }
}