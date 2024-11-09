using UserService.Models.Profile.Requests;
using UserService.Models.Profile.Responses;

namespace UserService.Services.Profile;

public interface IProfileService
{
    Task<GetProfileResponse> GetMyProfile(long userId);
    Task<UpdateProfileResponse> UpdateProfile(long  userId, UpdateProfileRequest request);
    Task<UploadAvatarResponse> UploadAvatar(long userId, UploadAvatarRequest request);
    Task<GetUsernameAndAvatarResponse> GetUsernameAndAvatar(long userId);
}