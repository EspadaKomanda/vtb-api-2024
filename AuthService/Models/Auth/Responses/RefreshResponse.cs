namespace AuthService.Models.Auth.Responses;

public class RefreshResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}