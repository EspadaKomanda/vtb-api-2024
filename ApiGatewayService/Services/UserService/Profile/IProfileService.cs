using ApiGatewayService.Models.UserService.Profile.Requests;
using UserService.Models.Profile.Requests;
using UserService.Models.Profile.Responses;

namespace ApiGatewayService.Services.UserService.Profile
{
    public interface IProfileService
    {
        public Task<GetProfileResponse> GetProfile(GetProfileRequest getProfileRequest);
        public Task<UpdateProfileResponse> UpdateProfile(UpdateProfileRequest updateProfileRequest);
        public Task<UploadAvatarResponse> UploadAvatar(UploadAvatarRequest uploadAvatarRequest);
        public Task<GetUsernameAndAvatarResponse> GetUsernameAndAvatar(GetUsernameAndAvatarRequest getUsernameAndAvatarRequest);
    }
}
