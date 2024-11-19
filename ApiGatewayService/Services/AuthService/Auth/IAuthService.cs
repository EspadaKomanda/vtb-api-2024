using ApiGatewayService.Models.AuthService.Authentication.Requests;
using ApiGatewayService.Models.AuthService.Authentication.Responses;

namespace ApiGatewayService.Services.AuthService.Auth;

public interface IAuthService
{
    public Task<LoginResponse> Login(LoginRequest request);
    public Task<ValidateAccessTokenResponse> ValidateAccessToken(ValidateAccessTokenRequest request);
    public Task<ValidateRefreshTokenResponse> ValidateRefreshToken(ValidateRefreshTokenRequest request);
    public Task<RefreshResponse> Refresh(RefreshRequest request);
}