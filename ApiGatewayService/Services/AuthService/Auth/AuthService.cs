using ApiGatewayService.Models.AuthService.Authentication.Requests;
using ApiGatewayService.Models.AuthService.Authentication.Responses;
using ApiGatewayService.Services.UserService.Account;
using TourService.Kafka;

namespace ApiGatewayService.Services.AuthService.Auth;

public class AuthService(ILogger<AccountService> logger, KafkaRequestService kafkaRequestService) : IAuthService
{
    private readonly ILogger<AccountService> _logger = logger;
    private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;

    public Task<RefreshResponse> Refresh(RefreshRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ValidateAccessTokenResponse> ValidateAccessToken(ValidateAccessTokenRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ValidateRefreshTokenResponse> ValidateRefreshToken(ValidateRefreshTokenRequest request)
    {
        throw new NotImplementedException();
    }
}
