using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using EntertaimentService.Models.User.Requests;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;
using UserService.Models;
using UserService.Models.Profile.Requests;
using UserService.Models.Profile.Responses;
using UserService.Services.Profile;

namespace UserService.KafkaServices
{
    public class ProfileKafkaService : KafkaService
    {
        private readonly string _profileResponseTopic = Environment.GetEnvironmentVariable("PROFILE_RESPONSE_TOPIC") ?? "profileServiceAccountsRequests";
        private readonly string _profileRequestTopic = Environment.GetEnvironmentVariable("PROFILE_REQUEST_TOPIC") ?? "profileServiceAccountsRequests";
        private readonly IProfileService _profileService;
        public ProfileKafkaService(
            ILogger<KafkaService> logger,
            IProfileService profileService,
            IProducer<string,string> producer,
            KafkaTopicManager kafkaTopicManager) : base(logger, producer, kafkaTopicManager)
        {
            _profileService = profileService;
            base.ConfigureConsumer(_profileRequestTopic);
        }

        public override async Task Consume()
        {
            try
            {
               
                while (true)
                {
                    if(_consumer == null)
                    {
                        _logger.LogError("Consumer is null");
                        throw new ConsumerException("Consumer is null");
                    }
                    ConsumeResult<string, string> consumeResult = _consumer.Consume();
                    if (consumeResult != null)
                    {
                        var headerBytes = consumeResult.Message.Headers
                        .FirstOrDefault(x => x.Key.Equals("method")) ?? throw new NullReferenceException("headerBytes is null");
                    
                  
                        var methodString = Encoding.UTF8.GetString(headerBytes.GetValueBytes());
                        switch (methodString)
                        {
                            case "getProfile":
                                try
                                {
                                    if(await base.Produce(_profileResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _profileService.GetMyProfile(JsonConvert.DeserializeObject<GetProfileRequest>(consumeResult.Message.Value).UserId)),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("getProfile")),
                                            new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                        ]
                                    }))
                                    {

                                        _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                        _consumer.Commit(consumeResult);
                                    }
                                }
                                catch (Exception e)
                                {
                                    if(e is MyKafkaException)
                                    {
                                        _logger.LogError(e,"Error sending message");
                                        throw;
                                    }
                                     _ = await base.Produce(_profileResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getProfile")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "updateProfile":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<UpdateProfileRequest>(consumeResult.Message.Value);
                                    if(await base.Produce(_profileResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _profileService.UpdateProfile(request.UserId, request)),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("updateProfile")),
                                            new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                        ]
                                    }))
                                    {
                                        _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                        _consumer.Commit(consumeResult);
                                    }
                                }
                                catch (Exception e)
                                {
                                    if(e is MyKafkaException)
                                    {
                                        _logger.LogError(e,"Error sending message");
                                        throw;
                                    }
                                    _ = await base.Produce(_profileResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("updateProfile")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "uploadAvatar":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<UploadAvatarRequest>(consumeResult.Message.Value);
                                    if(await base.Produce(_profileResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _profileService.UploadAvatar(request.UserId, request)),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("uploadAvatar")),
                                            new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                        ]
                                    }))
                                    {
                                        _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                        _consumer.Commit(consumeResult);
                                    }
                                }
                                catch (Exception e)
                                {
                                    if(e is MyKafkaException)
                                    {
                                        _logger.LogError(e,"Error sending message");
                                        throw;
                                    }
                                    _ = await base.Produce(_profileResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("uploadAvatar")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "getUsernameAvatar":
                                try
                                {
                                    var profile = await _profileService.GetMyProfile(JsonConvert.DeserializeObject<GetUserName>(consumeResult.Message.Value).UserId);
                                    if(await base.Produce(_profileResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( new GetUsernameAndAvatarResponse(){
                                            Username = profile.Name,
                                            Avatar = profile.Avatar
                                        }),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("getUsernameAvatar")),
                                            new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                        ]
                                    }))
                                    {
                                        _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                        _consumer.Commit(consumeResult);
                                    }
                                }
                                catch (Exception e)
                                {
                                    if(e is MyKafkaException)
                                    {
                                        _logger.LogError(e,"Error sending message");
                                        throw;
                                    }
                                    _ = await base.Produce(_profileResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getUsernameAvatar")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            
                            default: 
                                _consumer.Commit(consumeResult);
                                
                                throw new ConsumerRecievedMessageInvalidException("Invalid message received");
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                if(_consumer != null)
                { 
                    _consumer.Dispose();
                }
                if (ex is MyKafkaException)
                {
                    _logger.LogError(ex,"Consumer error");
                    throw new ConsumerException("Consumer error ",ex);
                }
                else
                {
                    _logger.LogError(ex,"Unhandled error");
                    throw;
                }
            }
        }
    }
}