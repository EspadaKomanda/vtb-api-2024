using UserService.Database.Models;
using UserService.Exceptions.Account;
using UserService.Models.Account.Requests;
using UserService.Models.Account.Responses;
using UserService.Repository;

namespace UserService.Services.Account;

public class AccountService(IRepository<User> userRepo, ILogger<AccountService> logger) : IAccountService
{
    private readonly IRepository<User> _userRepo = userRepo;
    private readonly ILogger<AccountService> _logger = logger;

    public async Task<AccountAccessDataResponse> AccountAccessData(AccountAccessDataRequest request)
    {
        if ((request.Email ?? request.Username) is null)
        {
            _logger.LogError("No user identification was provided for account search");
            throw new InsufficientDataException("Email or username must be provided");
        }

        User user;
        try 
        {
            user = await _userRepo.FindOneAsync(u => u.Email == request.Email || u.Username == request.Username);
            _logger.LogDebug($"Found user {user.Id} with email {request.Email} or username {request.Username}");
        }
        catch (NullReferenceException)
        {
            _logger.LogError($"No user with email {request.Email} or username {request.Username} found");
            throw new UserNotFoundException($"No user with email {request.Email} or username {request.Username} found");
        }

        _logger.LogDebug($"Returning account access data for user {user.Id}");
        return new AccountAccessDataResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Password = user.Password,
            Salt = user.Salt
        };
    }

    public Task<BeginPasswordResetResponse> BeginPasswordReset(BeginPasswordResetRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BeginRegistrationResponse> BeginRegistration(BeginRegistrationRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CompletePasswordResetResponse> CompletePasswordReset(CompletePasswordResetRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CompleteRegistrationResponse> CompleteRegistration(CompleteRegistrationRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResendPasswordResetCodeResponse> ResendPasswordResetCode(ResendPasswordResetCodeRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResendRegistrationCodeResponse> ResendRegistrationCode(ResendRegistrationCodeRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<VerifyPasswordResetCodeResponse> VerifyPasswordResetCode(VerifyPasswordResetCodeRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<VerifyRegistrationCodeResponse> VerifyRegistrationCode(VerifyRegistrationCodeRequest request)
    {
        throw new NotImplementedException();
    }
}