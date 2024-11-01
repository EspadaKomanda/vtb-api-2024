using UserService.Database.Models;
using UserService.Exceptions.Account;
using UserService.Models.Account.Requests;
using UserService.Models.Account.Responses;
using UserService.Repository;
using UserService.Utils;

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

    public async Task<BeginRegistrationResponse> BeginRegistration(BeginRegistrationRequest request)
    {
        User user;
        try 
        {
            user = await _userRepo.FindOneAsync(u => u.Email == request.Email || u.Username == request.Username);
            _logger.LogDebug($"Found user {user.Id} with email {request.Email} or username {request.Username}. Aborting registration");
            
            if (user.Email == request.Email)
                throw new UserExistsException($"User with email {request.Email} already exists");

            if (user.Username == request.Username)
                throw new UserExistsException($"User with username {request.Username} already exists");
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug($"Email {request.Email} and username {request.Username} are not taken, proceeding with registration");
        }

        // User creation
        user = new User
        {
            Email = request.Email,
            Username = request.Username,
            Password = request.Password,
            Salt = Guid.NewGuid().ToString()
        };

        try
        {
            await _userRepo.AddAsync(user);
            _logger.LogDebug($"Inserted user {user.Id} with email {request.Email} and username {request.Username}");
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed inserting user {user.Id} with email {request.Email} and username {request.Username}. {e}");
            throw;
        }

        // Registration code creation
        RegistrationCode regCode = new RegistrationCode
        {
            UserId = user.Id
        };
        try
        {
            await _regCodeRepo.AddAsync(regCode);
            _logger.LogDebug($"Inserted registration code for user {user.Id}");
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed inserting registration code for user {user.Id}. {e}");
            throw;
        }

        // TODO: Send email
        _logger.LogWarning($"Mailing backend is not yet implemented, registration code for user {user.Id} was not sent");

        _logger.LogDebug($"Registration code for user {user.Id} sent");

        _logger.LogDebug("Replying with success");
        return new BeginRegistrationResponse
        {
            IsSuccess = true
        };
    }

    public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request, long userId)
    {
        User user;
        try 
        {
            user = await _userRepo.FindOneAsync(u => u.Id == userId);
            _logger.LogDebug($"Found user {user.Id} with email {user.Email}");
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug($"No user with id {userId} found to change password");
            throw new UserNotFoundException($"No user with id {userId} found");
        }

        // Verify old password
        var inputOldPasswordHash = BcryptUtils.HashPassword(request.OldPassword);
        if (!BcryptUtils.VerifyPassword(request.OldPassword, user.Password))
        {
            _logger.LogDebug($"Old password did not match for user {user.Id}");
        }

        // Update password
        user.Password = BcryptUtils.HashPassword(request.NewPassword);
        try
        {
            _userRepo.Update(user);
            _logger.LogDebug($"Updated password for user {user.Id}");
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed updating password for user {user.Id}. {e}");
            throw;
        }

        // TODO: Ask AuthService and ApiGateway to recache information
        _logger.LogWarning($"Warning! UserService MUST notify AuthService and ApiGateway to recache information for user {user.Id}, but this feature is not yet implemented!");

        // TODO: Send notification email
        _logger.LogWarning($"Mailing backend is not yet implemented, notification for user {user.Id} was not sent");

        _logger.LogDebug("Replying with success");
        return new ChangePasswordResponse
        {
            IsSuccess = true
        };
    }

    public async Task<CompletePasswordResetResponse> CompletePasswordReset(CompletePasswordResetRequest request)
    {
        User user;
        ResetCode resetCode;
        try 
        {
            user = await _userRepo.FindOneAsync(u => u.Email == request.Email);
            resetCode = await _resetCodeRepo.FindOneAsync(rc => rc.Code == request.Code);
            _logger.LogDebug($"Found user {user.Id} with email {request.Email} and respective resetCode {resetCode.Id}");
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug($"No user with email {request.Email} or reset code {request.Code} found");
            throw new InvalidCodeException($"Invalid email or code");
        }

        // Update password
        user.Password = BcryptUtils.HashPassword(request.NewPassword);
        user.Salt = Guid.NewGuid().ToString();
        try
        {
            // FIXME: Transaction must be implemented to prevent errors
            _userRepo.Update(user);
            _resetCodeRepo.Delete(resetCode);
            _logger.LogDebug($"Updated password for user {user.Id}");
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed updating password for user {user.Id}. {e}");
            throw;
        }

        // TODO: Ask AuthService and ApiGateway to recache information
        _logger.LogWarning($"Warning! UserService MUST notify AuthService and ApiGateway to recache information for user {user.Id}, but this feature is not yet implemented!");

        // TODO: Send notification email
        _logger.LogWarning($"Mailing backend is not yet implemented, notification for user {user.Id} was not sent");

        _logger.LogDebug("Replying with success");
        return new CompletePasswordResetResponse
        {
            IsSuccess = true
        };
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