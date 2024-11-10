namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class GetUserException : System.Exception
    {
        public GetUserException() { }
        public GetUserException(string message) : base(message) { }
        public GetUserException(string message, System.Exception inner) : base(message, inner) { }
    }
}