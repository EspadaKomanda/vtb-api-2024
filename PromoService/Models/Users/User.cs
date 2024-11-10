namespace PromoService.Models.Users;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public DateTime CreationDate { get; set; }
}