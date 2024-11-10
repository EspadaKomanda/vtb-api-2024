using ApiGatewayService.Models.UserService.Profile.Requests;
using ApiGatewayService.Models.UserService.Profile.Responses;

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
