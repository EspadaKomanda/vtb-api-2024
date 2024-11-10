namespace ApiGatewayService.Exceptions.UserService.Profile;

[System.Serializable]
public class GetUsernameAndAvatarException : System.Exception
{
    public GetUsernameAndAvatarException() { }
    public GetUsernameAndAvatarException(string message) : base(message) { }
    public GetUsernameAndAvatarException(string message, System.Exception inner) : base(message, inner) { }
}