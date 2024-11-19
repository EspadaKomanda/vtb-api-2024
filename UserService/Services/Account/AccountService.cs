using System.Text;
using AuthService.Models.AccessDataCache.Requests;
using AuthService.Models.AccessDataCache.Responses;
using Confluent.Kafka;
using MailService.Models.Mail.Requests;
using MailService.Models.Mail.Responses;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.KafkaException.ConsumerException;
using UserService.Database.Models;
using UserService.Exceptions.Account;
using UserService.Models.Account.Requests;
using UserService.Models.Account.Responses;
using UserService.Repositories;
using UserService.Utils;

namespace UserService.Services.Account;

public class AccountService(IUnitOfWork unitOfWork, ILogger<AccountService> logger, KafkaRequestService kafkaService) : IAccountService
{
    private readonly IUnitOfWork _uow = unitOfWork;
    private readonly ILogger<AccountService> _logger = logger;
    private readonly KafkaRequestService _kafkaService = kafkaService;

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
            user = await _uow.Users.FindOneAsync(u => u.Email == request.Email || u.Username == request.Username);
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
            user = await _uow.Users.FindOneAsync(u => u.Email == request.Email);
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
            existingCode = await _uow.ResetCodes.FindOneAsync(rc => rc.UserId == user.Id);
            _logger.LogDebug("Found existing reset code for user {user.Id}", user.Id);

            if (existingCode.ExpirationDate < DateTime.UtcNow)
            {
                _logger.LogDebug("Reset code for user {user.Id} expired, updating", user.Id);
                
                existingCode.ExpirationDate = DateTime.UtcNow.AddMinutes(10);
                existingCode.Code = Guid.NewGuid().ToString();

                using var transaction = _uow.BeginTransaction();
                try
                {
                    _uow.ResetCodes.Update(existingCode);
                    transaction.SaveAndCommit();
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed updating reset code for user {user.Id}. {e}", user.Id, e);
                    transaction.Rollback();
                    throw;
                }
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

        var mailRequest = new SendMailRequest()
        {
            Body = $"Reset code: {existingCode.Code}",
            ToAddress = user.Email,
            IsDummy = true,
            Subject = "Password reset",
            IsHtml = false
        };
        var response = await SendEmail(mailRequest);
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
            user = await _uow.Users.FindOneAsync(u => u.Email == request.Email || u.Username == request.Username);
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

        using var transaction = _uow.BeginTransaction();
        
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
            await _uow.Users.AddAsync(user);
            if(_uow.Save()>=0)
            {
                _logger.LogDebug("Inserted user {user.Id} with email {request.Email} and username {request.Username}", user.Id, request.Email, request.Username);
       
            }
            else
            {

                throw new Exception("Error adding user!");
            }
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
            await _uow.RegistrationCodes.AddAsync(regCode);
            transaction.SaveAndCommit();
            _logger.LogDebug("Inserted registration code for user {user.Id}", user.Id);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed inserting registration code for user {user.Id}. {e}", user.Id, e);
            transaction.Rollback();
            throw;
        }
        

        var mailRequest = new SendMailRequest()
        {
            Body = $"Registration code: {regCode.Code}",
            ToAddress = user.Email,
            IsDummy = true,
            Subject = "Registration code",
            IsHtml = false
        };
        var response = await SendEmail(mailRequest);
        _logger.LogDebug("Registration code for user {user.Id} sent", user.Id);

        _logger.LogDebug("Replying with success");
        return new BeginRegistrationResponse
        {
            IsSuccess = true
        };
    }

    public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request)
    {
        User user;
        try 
        {
            user = await _uow.Users.FindOneAsync(u => u.Id == request.UserId);
            _logger.LogDebug("Found user {user.Id} with email {user.Email}", user.Id, user.Email);
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with id {userId} found to change password", request.UserId);
            throw new UserNotFoundException($"No user with id {request.UserId} found");
        }

        // Verify old password
        var inputOldPasswordHash = BcryptUtils.HashPassword(request.OldPassword);
        if (!BcryptUtils.VerifyPassword(request.OldPassword, user.Password))
        {
            _logger.LogDebug("Old password did not match for user {user.Id}", user.Id);
        }

        // Update password
        user.Password = BcryptUtils.HashPassword(request.NewPassword);
        using var transaction = _uow.BeginTransaction();
        try
        {
            _uow.Users.Update(user);
            transaction.SaveAndCommit();
            _logger.LogDebug("Updated password for user {user.Id}", user.Id);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed updating password for user {user.Id}. {e}", user.Id, e);
            transaction.Rollback();
            throw;
        }

        if(RecacheInfo(new RecacheUserRequest(){
            Password = user.Password,
            Salt = user.Salt,
            Username = user.Username,
            Role = user.Role.Name,

        }).Result.IsSuccess)
        {
            _logger.LogWarning("Mailing backend is not yet implemented, notification for user {user.Id} was not sent", user.Id);
            var EmailRequest = new SendMailRequest()
            {
                Body = "Password changed",
                ToAddress = user.Email,
                IsDummy = true,
                Subject = "Password changed",
                IsHtml = false
            };
            var result = await SendEmail(EmailRequest);
            _logger.LogDebug("Replying with success");
            return new ChangePasswordResponse
            {
                IsSuccess = true
            };
        }
        _logger.LogWarning("Warning! UserService MUST notify AuthService and ApiGateway to recache information for user {user.Id}, but this feature is not yet implemented!", user.Id);

        throw new Exception("Failed to recache user information");
    }

    public async Task<CompletePasswordResetResponse> CompletePasswordReset(CompletePasswordResetRequest request)
    {
        User user;
        ResetCode resetCode;
        try 
        {
            user = await _uow.Users.FindOneAsync(u => u.Email == request.Email);
            resetCode = await _uow.ResetCodes.FindOneAsync(rc => rc.Code == request.Code);
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

        using var transaction = _uow.BeginTransaction();
        try
        {
            _uow.Users.Update(user);
            _uow.ResetCodes.Delete(resetCode);
            transaction.SaveAndCommit();

            _logger.LogDebug("Updated password for user {user.Id}", user.Id);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed updating password for user {user.Id}. {e}", user.Id, e);
            transaction.Rollback();
            throw;
        }


        if(RecacheInfo(new RecacheUserRequest(){
            Password = user.Password,
            Salt = user.Salt,
            Username = user.Username,
            Role = user.Role.Name,

        }).Result.IsSuccess)
        {
            _logger.LogWarning("Warning! UserService MUST notify AuthService and ApiGateway to recache information for user {user.Id}, but this feature is not yet implemented!", user.Id);


            var mailRequest = new SendMailRequest()
            {
                Body = "Your password has been reset",
                ToAddress = user.Email,
                Subject = "Password reset",
                IsHtml = false,
                IsDummy = true
            };
            var response = await SendEmail(mailRequest);
            _logger.LogDebug("Replying with success");
            return new CompletePasswordResetResponse
            {
                IsSuccess = true
            };
        }
        _logger.LogWarning("Warning! UserService MUST notify AuthService and ApiGateway to recache information for user {user.Id}, but this feature is not yet implemented!", user.Id);
        throw new Exception("Failed to recache user information");
    }

    public async Task<CompleteRegistrationResponse> CompleteRegistration(CompleteRegistrationRequest request)
    {
        User user;
        RegistrationCode regCode;

        try 
        {
            user = await _uow.Users.FindOneAsync(u => u.Email == request.Email);

            regCode = await _uow.RegistrationCodes.FindOneAsync(rc => rc.Code == request.RegistrationCode && rc.UserId == user.Id);
            _logger.LogDebug("Found user {user.Id} with email {request.Email} and respective registrationCode {regCode.Id}", user.Id, request.Email, regCode.Id);
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with email {request.Email} or registration code {equest.RegistrationCode} found", request.Email, request.RegistrationCode);
            throw new InvalidCodeException($"Invalid email or code");
        }

        using (var transaction = _uow.BeginTransaction())
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
                    await _uow.Metas.AddAsync(meta);
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
                    await _uow.PersonalDatas.AddAsync(personalData);
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
                    _uow.Users.Update(user);
                    _uow.RegistrationCodes.Delete(regCode);
                    _logger.LogDebug("Updated user {user.Id}", user.Id);
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed activating user {user.Id}. {e}", user.Id, e);
                    throw;
                }

                transaction.SaveAndCommit();
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

    public async Task<GetUserResponse> GetUser(long userId, GetUserRequest request)
    {
        User user;
        Meta profile;
        try 
        {
            user = await _uow.Users.FindOneAsync(u => u.Id == userId);
            profile = await _uow.Metas.FindOneAsync(p => p.UserId == userId);
            _logger.LogDebug("Found user {user.Id} with profile {profile.Id}", user.Id, profile.Id);
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with id {userId} found", userId);
            throw new UserNotFoundException($"No user with id {userId} found");
        }
        return new GetUserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Avatar = profile.Avatar,
        };
    }

    public async Task<ResendPasswordResetCodeResponse> ResendPasswordResetCode(ResendPasswordResetCodeRequest request)
    {
        User user;
        ResetCode resetCode;
        try 
        {
            user = await _uow.Users.FindOneAsync(u => u.Email == request.Email);
            resetCode = await _uow.ResetCodes.FindOneAsync(rc => rc.UserId == user.Id);
            _logger.LogDebug("Found reset code {resetCode.Id} for user {user.Id}", resetCode.Id, user.Id);
            
            if (resetCode.ExpirationDate > DateTime.Now)
            {
                // Code is not yet expired
                _logger.LogDebug("Reset code for user {user.Id} is not yet expired", user.Id);
                throw new CodeHasNotExpiredException("Please, wait at least 10 minutes.");
            }
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with email {request.Email} found", request.Email);
            throw new UserNotFoundException($"No user with email {request.Email} found");
        }

        using var transaction = _uow.BeginTransaction();
        try
        {
            // Regenerate code
            resetCode.Code = Guid.NewGuid().ToString();
            resetCode.ExpirationDate = DateTime.UtcNow.AddMinutes(10);
            _uow.ResetCodes.Update(resetCode);
            transaction.SaveAndCommit();
            _logger.LogDebug("Updated reset code {resetCode.Id} for user {user.Id}", resetCode.Id, user.Id);

            var SendMailRequest = new SendMailRequest()
            {
                Body = $"Your password reset code: {resetCode.Code}",
                IsDummy = true,
                IsHtml = false,
                ToAddress = user.Email,
                Subject = "Password reset code"
            };
            var result = SendEmail(SendMailRequest);
            _logger.LogWarning("Mail service is not implemented, skipping sending password reset email with code {resetCode.Code} to user {user.Id}", resetCode.Code, user.Id);

            return new ResendPasswordResetCodeResponse
            {
                IsSuccess = true
            };
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError("Failed resending password reset code for user {user.Id}. {e}", user.Id, e);

            throw;
        }
    }

    public async Task<ResendRegistrationCodeResponse> ResendRegistrationCode(ResendRegistrationCodeRequest request)
    {
        User user;
        RegistrationCode regCode;
        try 
        {
            user = await _uow.Users.FindOneAsync(u => u.Email == request.Email);
            regCode = await _uow.RegistrationCodes.FindOneAsync(rc => rc.UserId == user.Id);
            _logger.LogDebug("Found registration code {regCode.Id} for user {user.Id}", regCode.Id, user.Id);
            
            if (regCode.ExpirationDate > DateTime.Now.AddMinutes(9))
            {
                // Code is not yet expired
                _logger.LogDebug("Registration code for user {user.Id} is not yet expired", user.Id);
                throw new CodeHasNotExpiredException("Please, wait at least 1 minute.");
            }
        }
        catch (NullReferenceException)
        {
            _logger.LogDebug("No user with email {request.Email} found", request.Email);
            throw new UserNotFoundException($"No user with email {request.Email} found");
        }

        using var transaction = _uow.BeginTransaction();
        try
        {
            // Regenerate code
            // FIXME: Match the six-character registration code template
            regCode.Code = Guid.NewGuid().ToString();
            regCode.ExpirationDate = DateTime.UtcNow.AddMinutes(10);
            _uow.RegistrationCodes.Update(regCode);
            transaction.SaveAndCommit();
            _logger.LogDebug("Updated registration code {regCode.Id} for user {user.Id}", regCode.Id, user.Id);

            var SendMailRequest = new SendMailRequest()
            {
                Body = $"Your registration code: {regCode.Code}",
                IsDummy = true,
                IsHtml = false,
                ToAddress = user.Email,
                Subject = "Registration code"
            };
            var result = SendEmail(SendMailRequest);
            _logger.LogWarning("Mail service is not implemented, skipping sending registration email with code {regCode.Code} to user {user.Id}", regCode.Code, user.Id);

            return new ResendRegistrationCodeResponse
            {
                IsSuccess = true
            };
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError("Failed resending registration code for user {user.Id}. {e}", user.Id, e);

            throw;
        }
    }

    public async Task<VerifyPasswordResetCodeResponse> VerifyPasswordResetCode(VerifyPasswordResetCodeRequest request)
    {
        try 
        {
            User user = await _uow.Users.FindOneAsync(u => u.Email == request.Email);
            ResetCode resetCode = await _uow.ResetCodes.FindOneAsync(rc => rc.Code == request.Code);
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
            User user = await _uow.Users.FindOneAsync(u => u.Email == request.Email);
            RegistrationCode regCode = await _uow.RegistrationCodes.FindOneAsync(rc => rc.Code == request.Code);
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
    private async Task<SendMailResponse> SendEmail(SendMailRequest request)
    {
        try
        {
            Guid messageId = Guid.NewGuid();
            Message<string,string> message = new Message<string, string>()
            {
                Key = messageId.ToString(),
                Value = JsonConvert.SerializeObject(request),
                Headers = new Headers()
                {
                    new Header("method",Encoding.UTF8.GetBytes("sendMail")),
                    new Header("sender",Encoding.UTF8.GetBytes("mailService"))
                }
            };
            if(await _kafkaService.Produce(Environment.GetEnvironmentVariable("MAIL_REQUEST_TOPIC"),message,Environment.GetEnvironmentVariable("MAIL_RESPONSE_TOPIC")))
            {
                _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                while (!_kafkaService.IsMessageRecieved(messageId.ToString()))
                {
                    Thread.Sleep(200);
                }
                _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                return _kafkaService.GetMessage<SendMailResponse>(messageId.ToString(),Environment.GetEnvironmentVariable("MAIL_RESPONSE_TOPIC"));
            }
            throw new ConsumerException("Message not recieved");
        }
        catch (Exception e)
        {
            _logger.LogError("Failed sending email. {e}", e);
            throw;
        }
    }
    private async Task<RecacheUserResponse> RecacheInfo(RecacheUserRequest request)
    {
        try
        {
            Guid messageId = Guid.NewGuid();
            Message<string,string> message = new Message<string, string>()
            {
                Key = messageId.ToString(),
                Value = JsonConvert.SerializeObject(request),
                Headers = new Headers()
                {
                    new Header("method",Encoding.UTF8.GetBytes("recacheUser")),
                    new Header("sender",Encoding.UTF8.GetBytes("authService"))
                }
            };
            if(await _kafkaService.Produce(Environment.GetEnvironmentVariable("DATA_CACHE_REQUEST_TOPIC"),message,Environment.GetEnvironmentVariable("DATA_CACHE_RESPONSE_TOPIC")))
            {
                _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                while (!_kafkaService.IsMessageRecieved(messageId.ToString()))
                {
                    Thread.Sleep(200);
                }
                _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                return _kafkaService.GetMessage<RecacheUserResponse>(messageId.ToString(),Environment.GetEnvironmentVariable("DATA_CACHE_RESPONSE_TOPIC"));
            }
            throw new ConsumerException("Message not recieved");
        }
        catch (Exception e)
        {
            _logger.LogError("Failed sending recache. {e}", e);
            throw;
        }
    }
}