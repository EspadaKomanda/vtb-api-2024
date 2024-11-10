namespace ApiGatewayService.Exceptions.UserService.Profile;

[System.Serializable]
public class UpdateProfileException : System.Exception
{
    public UpdateProfileException() { }
    public UpdateProfileException(string message) : base(message) { }
    public UpdateProfileException(string message, System.Exception inner) : base(message, inner) { }
}