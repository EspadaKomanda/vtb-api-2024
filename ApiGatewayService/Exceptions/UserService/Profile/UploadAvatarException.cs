namespace ApiGatewayService.Exceptions.UserService.Profile;

[System.Serializable]
public class UploadAvatarException : System.Exception
{
    public UploadAvatarException() { }
    public UploadAvatarException(string message) : base(message) { }
    public UploadAvatarException(string message, System.Exception inner) : base(message, inner) { }
}