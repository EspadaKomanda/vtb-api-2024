namespace ApiGatewayService.Models.AuthService.Authentication.Requests;

public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}