using ApiGatewayService.Models.UserService.Profile.Requests;
using ApiGatewayService.Models.UserService.Profile.Responses;

namespace ApiGatewayService.Services.UserService.Profile;

public class ProfileService : IProfileService
{
    public Task<GetProfileResponse> GetProfile(GetProfileRequest getProfileRequest)
    {
        throw new NotImplementedException();
    }

    public Task<GetUsernameAndAvatarResponse> GetUsernameAndAvatar(GetUsernameAndAvatarRequest getUsernameAndAvatarRequest)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateProfileResponse> UpdateProfile(UpdateProfileRequest updateProfileRequest)
    {
        throw new NotImplementedException();
    }

    public Task<UploadAvatarResponse> UploadAvatar(UploadAvatarRequest uploadAvatarRequest)
    {
        throw new NotImplementedException();
    }
}
