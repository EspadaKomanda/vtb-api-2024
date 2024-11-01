using UserService.Database.Models;
using UserService.Exceptions.Account;
using UserService.Models.Account.Requests;
using UserService.Models.Account.Responses;
using UserService.Repository;

namespace UserService.Services.Account;

public class AccountService(IRepository<User> userRepo, IRepository<RegistrationCode> regCodeRepo, IRepository<ResetCode> resetCodeRepo, ILogger<AccountService> logger) : IAccountService
{
    private readonly IRepository<User> _userRepo = userRepo;
    private readonly IRepository<RegistrationCode> _regCodeRepo = regCodeRepo;
    private readonly IRepository<ResetCode> _resetCodeRepo = resetCodeRepo;
    private readonly ILogger<AccountService> _logger = logger;

    public async Task<AccountAccessDataResponse> AccountAccessData(AccountAccessDataRequest request)
    {
        if ((request.Email ?? request.Username) is null)
        {
            _logger.LogDebug("No user identification was provided for account search");
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
            _logger.LogDebug($"No user with email {request.Email} or username {request.Username} found");
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

    public async Task<BeginPasswordResetResponse> BeginPasswordReset(BeginPasswordResetRequest request)
    {
        User user;
        try 
        {
            user = await _userRepo.FindOneAsync(u => u.Email == request.Email);
            _logger.LogDebug($"Found user {user.Id} with email {request.Email}");
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug($"No user with email {request.Email} found to start password reset procedure");
            throw new UserNotFoundException($"No user with email {request.Email} found");
        }

        ResetCode existingCode;
        try 
        {
            existingCode = await _resetCodeRepo.FindOneAsync(rc => rc.UserId == user.Id);
            _logger.LogDebug($"Found existing reset code for user {user.Id}");

            if (existingCode.ExpirationDate < DateTime.UtcNow)
            {
                _logger.LogDebug($"Reset code for user {user.Id} expired, updating");
                
                existingCode.ExpirationDate = DateTime.UtcNow.AddMinutes(10);
                existingCode.Code = Guid.NewGuid().ToString();
                _resetCodeRepo.Update(existingCode);
            }
            else
            {
                _logger.LogDebug($"Reset code for user {user.Id} not expired");
                throw new CodeHasNotExpiredException($"Reset code for user {user.Id} already exists and is not expired");
            }
        }
        catch (Exception e) when (e is not CodeHasNotExpiredException)
        {
            _logger.LogError("Failed retrieving/updating reset code for user {user.Id}", user.Id);
            throw;
        }

        // TODO: Send email
        _logger.LogWarning($"Mailing backend is not yet implemented, reset code {existingCode.Code} for user {user.Id} was not sent");

        _logger.LogDebug($"Reset code for user {user.Id} sent");

        _logger.LogDebug("Replying with success");
        return new BeginPasswordResetResponse
        {
            IsSuccess = true
        };
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