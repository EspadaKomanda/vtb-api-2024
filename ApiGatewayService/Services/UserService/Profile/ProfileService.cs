using System.Text;
using ApiGatewayService.Models.UserService.Profile.Requests;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using UserService.Models.Profile.Requests;
using UserService.Models.Profile.Responses;

namespace ApiGatewayService.Services.UserService.Profile;

public class ProfileService(ILogger<ProfileService> logger, KafkaRequestService kafkaRequestService) : IProfileService
{
    private readonly ILogger<ProfileService> _logger = logger;
    private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
    private readonly string requestTopic = Environment.GetEnvironmentVariable("USER_SERVICE_PROFILE_REQUESTS");
    private readonly string responseTopic = Environment.GetEnvironmentVariable("USER_SERVICE_PROFILE_RESPONSES");
    private readonly string serviceName = "apiGatewayService";
    private async Task<Q> SendRequest<T,Q>(string methodName, T request)
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
                    new Header("method",Encoding.UTF8.GetBytes(methodName)),
                    new Header("sender",Encoding.UTF8.GetBytes(serviceName))
                }
            };
            if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
            {
                _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                {
                    Thread.Sleep(200);
                }
                _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                return _kafkaRequestService.GetMessage<Q>(messageId.ToString(),responseTopic);
            }
            throw new Exception("Message not recieved");
        }
        catch (Exception)
        {
            throw;
        }
    }
        
    public async Task<GetProfileResponse> GetProfile(GetProfileRequest getProfileRequest)
    {
        try
        {
            return await SendRequest<GetProfileRequest,GetProfileResponse>("getProfile",getProfileRequest);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Failed to get profile");
            throw;
        }
    }

    public async Task<GetUsernameAndAvatarResponse> GetUsernameAndAvatar(GetUsernameAndAvatarRequest getUsernameAndAvatarRequest)
    {
        try
        {
            return await SendRequest<GetUsernameAndAvatarRequest,GetUsernameAndAvatarResponse>("getUsernameAndAvatar",getUsernameAndAvatarRequest);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Failed to get username and avatar");
            throw;
        }
    }

    public async Task<UpdateProfileResponse> UpdateProfile(UpdateProfileRequest updateProfileRequest)
    {
        try
        {
            return await SendRequest<UpdateProfileRequest,UpdateProfileResponse>("updateProfile",updateProfileRequest);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Failed to update profile");
            throw;
        }
    }

    public async Task<UploadAvatarResponse> UploadAvatar(UploadAvatarRequest uploadAvatarRequest)
    {
        try
        {
            return await SendRequest<UploadAvatarRequest,UploadAvatarResponse>("uploadAvatar",uploadAvatarRequest);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Failed to upload avatar");
            throw;
        }
    }
}
