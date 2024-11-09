using UserService.Models.Profile.Requests;
using UserService.Models.Profile.Responses;

namespace UserService.Services.Profile;

public class ProfileService : IProfileService
{
    public Task<GetProfileResponse> GetMyProfile(long userId)
    {
        throw new NotImplementedException();
    }

    public UpdateProfileResponse UpdateProfile(UpdateProfileRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UploadAvatarResponse> UploadAvatar(UploadAvatarRequest request)
    {
        throw new NotImplementedException();
    }
}