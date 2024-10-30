namespace AuthService.Services.Auth;

public interface IAuthService
{
    Task<bool> Refresh(string refreshToken);
}