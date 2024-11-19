using EntertaimentService.Services.S3;
using EntertaimentService.Utils.Models;
using UserService.Database.Models;
using UserService.Exceptions.Account;
using UserService.Models.Profile.Requests;
using UserService.Models.Profile.Responses;
using UserService.Repositories;

namespace UserService.Services.Profile;

public class ProfileService(IUnitOfWork unitOfWork, ILogger<ProfileService> logger, IS3Service s3Service, IConfiguration configuration) : IProfileService
{
    private readonly IUnitOfWork _uow = unitOfWork;
    private readonly ILogger<ProfileService> _logger = logger;
    private readonly IS3Service _s3Service = s3Service;
    private readonly IConfiguration _configuration = configuration;
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

    public async Task<UploadAvatarResponse> UploadAvatar(long userId, UploadAvatarRequest request)
    {
        try
        {
            List<Bucket> buckets = _configuration.GetSection("Buckets").Get<List<Bucket>>() ?? throw new NullReferenceException();
            if(await _s3Service.UploadImageToS3Bucket(request.Avatar,buckets.FirstOrDefault(x=>x.BucketName=="AvatarImages")!.BucketId.ToString(),request.UserId.ToString()))
            {
                var photo =  await _s3Service.GetImageFromS3Bucket(request.UserId.ToString(),buckets.FirstOrDefault(x=>x.BucketName=="AvatarImages")!.BucketId.ToString());
                var meta = await _uow.Metas.FindOneAsync(p => p.UserId == userId);
                meta.Avatar = photo.FileLink;
                _uow.Metas.Update(meta);
                if(_uow.Save()>=0)
                {
                    return new UploadAvatarResponse
                    {
                        Url = photo.FileLink
                    };
                }
                throw new Exception("Failed to upload avatar!");
            }
            throw new Exception("Failed to upload avatar!");

        }
        catch (Exception e)
        {
            throw;
        }
    }
}