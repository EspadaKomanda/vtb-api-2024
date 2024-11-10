namespace UserService.Models.Account.Responses;

public class GetUserResponse
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string? Avatar { get; set; } = null!;
}