namespace ApiGatewayService.Models.Jwt;

public class ValidatedUser
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
}