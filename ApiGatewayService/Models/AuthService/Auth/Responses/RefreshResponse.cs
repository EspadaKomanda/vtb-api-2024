namespace ApiGatewayService.Models.AuthService.Authentication.Responses;

public class RefreshResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}