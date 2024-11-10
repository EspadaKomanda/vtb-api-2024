namespace ApiGatewayService.Models.Profile.Requests;

public class UpdateProfileRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Patronymic { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Avatar { get; set; }
}