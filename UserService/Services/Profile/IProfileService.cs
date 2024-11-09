using UserService.Models.Profile.Requests;
using UserService.Models.Profile.Responses;

namespace UserService.Services.Profile;

public interface IProfileService
{
    Task<GetProfileResponse> GetMyProfile(long userId);
    UpdateProfileResponse UpdateProfile(UpdateProfileRequest request);
    Task<UploadAvatarResponse> UploadAvatar(UploadAvatarRequest request);
}