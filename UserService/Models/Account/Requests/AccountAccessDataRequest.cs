namespace UserService.Models.Account.Requests;

/// <summary>
/// Запрос на получение данных для входа в аккаунт.
/// </summary>
public class AccountAccessDataRequest
{
    public string? Username { get; set; }
    public string? Email { get; set; }
}