using UserService.Database.Models;
using UserService.Exceptions.Account;
using UserService.Models.Account.Requests;
using UserService.Models.Account.Responses;
using UserService.Repositories;
using UserService.Utils;

namespace UserService.Services.Account;

public class AccountService(IRepository<User> userRepo, IRepository<RegistrationCode> regCodeRepo, IRepository<ResetCode> resetCodeRepo, IRepository<Meta> metaRepo, IRepository<PersonalData> personalDataRepo, IUnitOfWork unitOfWork, ILogger<AccountService> logger) : IAccountService
{
    private readonly IRepository<User> _userRepo = userRepo;
    private readonly IRepository<RegistrationCode> _regCodeRepo = regCodeRepo;
    private readonly IRepository<ResetCode> _resetCodeRepo = resetCodeRepo;
    private readonly IRepository<Meta> _metaRepo = metaRepo;
    private readonly IRepository<PersonalData> _personalDataRepo = personalDataRepo;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
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
            _logger.LogDebug("Found user {user.Id} with email {request.Email} or username {request.Username}", user.Id, request.Email, request.Username);
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with email {request.Email} or username {request.Username} found", request.Email, request.Username);
            throw new UserNotFoundException($"No user with email {request.Email} or username {request.Username} found");
        }

        _logger.LogDebug("Returning account access data for user {user.Id}", user.Id);
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
            _logger.LogDebug("Found user {user.Id} with email {request.Email}", user.Id, request.Email);
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with email {request.Email} found to start password reset procedure", request.Email);
            throw new UserNotFoundException($"No user with email {request.Email} found");
        }

        ResetCode existingCode;
        try 
        {
            existingCode = await _resetCodeRepo.FindOneAsync(rc => rc.UserId == user.Id);
            _logger.LogDebug("Found existing reset code for user {user.Id}", user.Id);

            if (existingCode.ExpirationDate < DateTime.UtcNow)
            {
                _logger.LogDebug("Reset code for user {user.Id} expired, updating", user.Id);
                
                existingCode.ExpirationDate = DateTime.UtcNow.AddMinutes(10);
                existingCode.Code = Guid.NewGuid().ToString();
                _resetCodeRepo.Update(existingCode);
            }
            else
            {
                _logger.LogDebug("Reset code for user {user.Id} not expired", user.Id);
                throw new CodeHasNotExpiredException($"Reset code for user {user.Id} already exists and is not expired");
            }
        }
        catch (Exception e) when (e is not CodeHasNotExpiredException)
        {
            _logger.LogError("Failed retrieving/updating reset code for user {user.Id}", user.Id);
            throw;
        }

        // TODO: Send email
        _logger.LogWarning("Mailing backend is not yet implemented, reset code {existingCode.Code} for user {user.Id} was not sent", existingCode.Code, user.Id);

        _logger.LogDebug("Reset code for user {user.Id} sent", user.Id);

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
            _logger.LogDebug("Found user {user.Id} with email {request.Email} or username {request.Username}. Aborting registration", user.Id, request.Email, request.Username);
            
            if (user.Email == request.Email)
                throw new UserExistsException($"User with email {request.Email} already exists");

            if (user.Username == request.Username)
                throw new UserExistsException($"User with username {request.Username} already exists");
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("Email {request.Email} and username {request.Username} are not taken, proceeding with registration", request.Email, request.Username);
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
            _logger.LogDebug("Inserted user {user.Id} with email {request.Email} and username {request.Username}", user.Id, request.Email, request.Username);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed inserting user {user.Id} with email {request.Email} and username {request.Username}. {e}", user.Id, request.Email, request.Username, e);
            throw;
        }

        // Registration code creation
        RegistrationCode regCode = new()
        {
            UserId = user.Id
        };
        try
        {
            await _regCodeRepo.AddAsync(regCode);
            _logger.LogDebug("Inserted registration code for user {user.Id}", user.Id);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed inserting registration code for user {user.Id}. {e}", user.Id, e);
            throw;
        }

        // TODO: Send email
        _logger.LogWarning("Mailing backend is not yet implemented, registration code for user {user.Id} was not sent", user.Id);

        _logger.LogDebug("Registration code for user {user.Id} sent", user.Id);

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
            _logger.LogDebug("Found user {user.Id} with email {user.Email}", user.Id, user.Email);
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with id {userId} found to change password", userId);
            throw new UserNotFoundException($"No user with id {userId} found");
        }

        // Verify old password
        var inputOldPasswordHash = BcryptUtils.HashPassword(request.OldPassword);
        if (!BcryptUtils.VerifyPassword(request.OldPassword, user.Password))
        {
            _logger.LogDebug("Old password did not match for user {user.Id}", user.Id);
        }

        // Update password
        user.Password = BcryptUtils.HashPassword(request.NewPassword);
        try
        {
            _userRepo.Update(user);
            _logger.LogDebug("Updated password for user {user.Id}", user.Id);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed updating password for user {user.Id}. {e}", user.Id, e);
            throw;
        }

        // TODO: Ask AuthService and ApiGateway to recache information
        _logger.LogWarning("Warning! UserService MUST notify AuthService and ApiGateway to recache information for user {user.Id}, but this feature is not yet implemented!", user.Id);

        // TODO: Send notification email
        _logger.LogWarning("Mailing backend is not yet implemented, notification for user {user.Id} was not sent", user.Id);

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
            _logger.LogDebug("Found user {user.Id} with email {request.Email} and respective resetCode {resetCode.Id}", user.Id, request.Email, resetCode.Id);
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with email {request.Email} or reset code {request.Code} found", request.Email, request.Code);
            throw new InvalidCodeException($"Invalid email or code");
        }

        // Update password
        user.Password = BcryptUtils.HashPassword(request.NewPassword);
        user.Salt = Guid.NewGuid().ToString();

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                _unitOfWork.Users.Update(user);
                _unitOfWork.ResetCodes.Delete(resetCode);
                _unitOfWork.Save();
                transaction.Commit();

                _logger.LogDebug("Updated password for user {user.Id}", user.Id);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed updating password for user {user.Id}. {e}", user.Id, e);
                transaction.Rollback();
                throw;
            }
        }

        // TODO: Ask AuthService and ApiGateway to recache information
        _logger.LogWarning("Warning! UserService MUST notify AuthService and ApiGateway to recache information for user {user.Id}, but this feature is not yet implemented!", user.Id);

        // TODO: Send notification email
        _logger.LogWarning("Mailing backend is not yet implemented, notification for user {user.Id} was not sent", user.Id);

        _logger.LogDebug("Replying with success");
        return new CompletePasswordResetResponse
        {
            IsSuccess = true
        };
    }

    public async Task<CompleteRegistrationResponse> CompleteRegistration(CompleteRegistrationRequest request)
    {
        User user;
        RegistrationCode regCode;

        try 
        {
            user = await _userRepo.FindOneAsync(u => u.Email == request.Email);

            regCode = await _regCodeRepo.FindOneAsync(rc => rc.Code == request.RegistrationCode && rc.UserId == user.Id);
            _logger.LogDebug("Found user {user.Id} with email {request.Email} and respective registrationCode {regCode.Id}", user.Id, request.Email, regCode.Id);
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with email {request.Email} or registration code {equest.RegistrationCode} found", request.Email, request.RegistrationCode);
            throw new InvalidCodeException($"Invalid email or code");
        }

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                // Create meta
                var meta = new Meta
                {
                    UserId = user.Id,
                    Name = request.Name,
                    Surname = request.Surname,
                    Patronymic = request.Patronymic,
                    Birthday = request.Birthday,
                    Avatar = request.Avatar
                };
                try
                {
                    await _unitOfWork.Metas.AddAsync(meta);
                    _logger.LogDebug("Created meta for user {user.Id}", user.Id);
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed creating meta for user {user.Id}. {e}", user.Id, e);
                    throw;
                }

                // Create personal data
                var personalData = new PersonalData
                {
                    UserId = user.Id
                };
                try
                {
                    await _unitOfWork.PersonalDatas.AddAsync(personalData);
                    _logger.LogDebug("Created personal data for user {user.Id}", user.Id);
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed creating personal data for user {user.Id}. {e}", user.Id, e);
                    throw;
                }

                user.IsActivated = true;
                try
                {
                    _unitOfWork.Users.Update(user);
                    _unitOfWork.RegistrationCodes.Delete(regCode);
                    _logger.LogDebug("Updated user {user.Id}", user.Id);
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed activating user {user.Id}. {e}", user.Id, e);
                    throw;
                }

                _unitOfWork.Save();
                transaction.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed completing registration for user {user.Id}. {e}", user.Id, e);
                transaction.Rollback();
                throw;
            }
        }

        return new CompleteRegistrationResponse
        {
            IsSuccess = true
        };
    }

    public async Task<ResendPasswordResetCodeResponse> ResendPasswordResetCode(ResendPasswordResetCodeRequest request)
    {
        // TODO: Email service needed
        try 
        {
            User user = await _userRepo.FindOneAsync(u => u.Email == request.Email);
            ResetCode resetCode = await _resetCodeRepo.FindOneAsync(rc => rc.UserId == user.Id);
            _logger.LogDebug("Found reset code {resetCode.Id} for user {user.Id}", resetCode.Id, user.Id);
            
            if (resetCode.ExpirationDate > DateTime.Now)
            {
                // Code is not yet expired
                _logger.LogDebug("Reset code for user {user.Id} is not yet expired", user.Id);
                throw new CodeHasNotExpiredException("Please, wait at least 10 minutes.");
            }

            // Regenerate code
            resetCode.Code = Guid.NewGuid().ToString();
            resetCode.ExpirationDate = DateTime.UtcNow.AddMinutes(10);
            _resetCodeRepo.Update(resetCode);

            return new ResendPasswordResetCodeResponse
            {
                IsSuccess = true
            };
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No reset code found for user {request.Email}", request.Email);
            throw new InvalidCodeException();
        }
    }

    public async Task<ResendRegistrationCodeResponse> ResendRegistrationCode(ResendRegistrationCodeRequest request)
    {
        // TODO: Email service needed
        try 
        {
            User user = await _userRepo.FindOneAsync(u => u.Email == request.Email);
            RegistrationCode regCode = await _regCodeRepo.FindOneAsync(rc => rc.UserId == user.Id);
            _logger.LogDebug("Found registration code {regCode.Id} for user {user.Id}", regCode.Id, user.Id);
            
            if (regCode.ExpirationDate > DateTime.Now.AddMinutes(9))
            {
                // Code is not yet expired
                _logger.LogDebug("Registration code for user {user.Id} is not yet expired", user.Id);
                throw new CodeHasNotExpiredException("Please, wait at least 1 minute before resending the code");
            }

            // Regenerate code
            regCode.Code = Guid.NewGuid().ToString();
            regCode.ExpirationDate = DateTime.UtcNow.AddMinutes(10);
            _regCodeRepo.Update(regCode);

            return new ResendRegistrationCodeResponse
            {
                IsSuccess = true
            };
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No registration code found for user {request.Email}", request.Email);
            throw new InvalidCodeException();
        }
    }

    public async Task<VerifyPasswordResetCodeResponse> VerifyPasswordResetCode(VerifyPasswordResetCodeRequest request)
    {
        try 
        {
            User user = await _userRepo.FindOneAsync(u => u.Email == request.Email);
            ResetCode resetCode = await _resetCodeRepo.FindOneAsync(rc => rc.Code == request.Code);
            _logger.LogDebug("Found reset code {resetCode.Id} for user {user.Id}", resetCode.Id, user.Id);
            
            return new VerifyPasswordResetCodeResponse
            {
                IsSuccess = true
            };
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No reset code {request.Code} found for user {request.Email}", request.Code, request.Email);
            throw new InvalidCodeException($"Invalid code");
        }
    }

    public async Task<VerifyRegistrationCodeResponse> VerifyRegistrationCode(VerifyRegistrationCodeRequest request)
    {
        try 
        {
            User user = await _userRepo.FindOneAsync(u => u.Email == request.Email);
            RegistrationCode regCode = await _regCodeRepo.FindOneAsync(rc => rc.Code == request.Code);
            _logger.LogDebug("Found reset code {regCode.Id} for user {user.Id}", regCode.Id, user.Id);
            
            return new VerifyRegistrationCodeResponse
            {
                IsSuccess = true
            };
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No registration code {request.Code} found for user {request.Email}", request.Code, request.Email);
            throw new InvalidCodeException($"Invalid code");
        }
    }
}