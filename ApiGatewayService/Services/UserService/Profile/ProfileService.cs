using ApiGatewayService.Models.UserService.Profile.Requests;
using ApiGatewayService.Models.UserService.Profile.Responses;
using TourService.Kafka;

namespace ApiGatewayService.Services.UserService.Profile;

public class ProfileService(ILogger<ProfileService> logger, KafkaRequestService kafkaRequestService) : IProfileService
{
    private readonly ILogger<ProfileService> _logger = logger;
    private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;

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
