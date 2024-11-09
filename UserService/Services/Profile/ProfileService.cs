using UserService.Database.Models;
using UserService.Exceptions.Account;
using UserService.Models.Profile.Requests;
using UserService.Models.Profile.Responses;
using UserService.Repositories;

namespace UserService.Services.Profile;

public class ProfileService(UnitOfWork unitOfWork, ILogger<ProfileService> logger) : IProfileService
{
    private readonly UnitOfWork _uow = unitOfWork;
    private readonly ILogger<ProfileService> _logger = logger;

    public async Task<GetProfileResponse> GetMyProfile(long userId)
    {
        Meta profile;
        try 
        {
            profile = await _uow.Metas.FindOneAsync(p => p.UserId == userId);
            _logger.LogDebug("Found profile for user {userId}", userId);
        }
        catch (Exception e)
        {
            _logger.LogDebug("Failed to acquire profile for user {userId}", userId);

            try 
            {
                User user = await _uow.Users.FindOneAsync(u => u.Id == userId);
            }
            catch (NullReferenceException)
            {
                _logger.LogDebug("No user with id {userId} found", userId);
                throw new UserNotFoundException($"No profile for user {userId} found");
            }
            throw;
        }
        return (GetProfileResponse)profile;
    }

    public async Task<GetUsernameAndAvatarResponse> GetUsernameAndAvatar(long userId)
    {
        User user;
        Meta profile;
        try
        {
            user = await _uow.Users.FindOneAsync(u => u.Id == userId);
            profile = await _uow.Metas.FindOneAsync(p => p.UserId == userId);
            _logger.LogDebug("Found profile for user {userId}", userId);

            return new GetUsernameAndAvatarResponse
            {
                Username = user.Username,
                Avatar = profile.Avatar
            };
        }
        catch (Exception e)
        {
            _logger.LogDebug("Failed to acquire profile for user {userId}", userId);
            try 
            {
                user = await _uow.Users.FindOneAsync(u => u.Id == userId);
            }
            catch (NullReferenceException)
            {
                _logger.LogDebug("No user with id {userId} found", userId);
                throw new UserNotFoundException($"No profile for user {userId} found");
            }
            throw;
        }
    }

    public async Task<UpdateProfileResponse> UpdateProfile(long userId, UpdateProfileRequest request)
    {
        Meta profile;
        using var transaction = _uow.BeginTransaction();
        try 
        {
            profile = await _uow.Metas.FindOneAsync(p => p.UserId == userId);
            _logger.LogDebug("Found profile for user {userId}", userId);

            profile.Name = request.Name ?? profile.Name;
            profile.Surname = request.Surname ?? profile.Surname;
            profile.Patronymic = request.Patronymic ?? profile.Patronymic;
            profile.Birthday = request.Birthday ?? profile.Birthday;
            profile.Avatar = request.Avatar ?? profile.Avatar;

            transaction.SaveAndCommit();

            _logger.LogDebug("Updated profile for user {userId}", userId);
        }
        catch (Exception e)
        {
            _logger.LogDebug("Failed to acquire profile for user {userId}", userId);
            transaction.Rollback();

            try 
            {
                User user = await _uow.Users.FindOneAsync(u => u.Id == userId);
            }
            catch (NullReferenceException)
            {
                _logger.LogDebug("No user with id {userId} found", userId);
                throw new UserNotFoundException($"No profile for user {userId} found");
            }
            throw;
        }
        return (UpdateProfileResponse)profile;
    }

    // TODO: Ereshkigal wants to implement this
    public async Task<UploadAvatarResponse> UploadAvatar(long userId, UploadAvatarRequest request)
    {
        throw new NotImplementedException();
    }
}