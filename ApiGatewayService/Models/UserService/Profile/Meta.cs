namespace ApiGatewayService.Models.UserService.Profile;

public class Meta
{
  public long Id { get; set; }
  public long UserId { get; set; }
  public string Name { get; set; } = null!;
  public string Surname { get; set; } = null!;
  public string? Patronymic { get; set; }
  public DateTime Birthday { get; set; }
  public string? Avatar { get; set; }
}