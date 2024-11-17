using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Models.Auth.Requests;
using AuthService.Models.Authentication.Requests;
using AuthService.Services.Authentication;
using Confluent.Kafka;
using EntertaimentService.Kafka.Utils;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;

namespace AuthService.KafkaServices
{
    public class KafkaAuthService : KafkaService
    {
        private readonly string _authRequestTopic = Environment.GetEnvironmentVariable("AUTH_REQUEST_TOPIC") ?? "authRequestTopic";
        private readonly string _authResponseTopic = Environment.GetEnvironmentVariable("AUTH_RESPONSE_TOPIC") ?? "authResponseTopic";
        private readonly IAuthenticationService _authenticationService;
        public KafkaAuthService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager,  IAuthenticationService authenticationService) : base(logger, producer, kafkaTopicManager)
        {
            _authenticationService = authenticationService;
            base.ConfigureConsumer(_authRequestTopic);
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
                            case "refreshToken":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<RefreshRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_authResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                await _authenticationService.Refresh(request)
                                            ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("refreshToken")),
                                                new Header("sender",Encoding.UTF8.GetBytes("authService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_authResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("refreshToken")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("authService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "validateAccessToken":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<ValidateAccessTokenRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_authResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                await _authenticationService.ValidateAccessToken(request)
                                            ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("validateAccessToken")),
                                                new Header("sender",Encoding.UTF8.GetBytes("authService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_authResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("validateAccessToken")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("authService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "validateRefreshToken":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<ValidateRefreshTokenRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_authResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                await _authenticationService.ValidateRefreshToken(request)
                                            ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("validateRefreshToken")),
                                                new Header("sender",Encoding.UTF8.GetBytes("authService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Request validation error");

                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_authResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("validateRefreshToken")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("authService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "login":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<LoginRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_authResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                await _authenticationService.Login(request)
                                            ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("login")),
                                                new Header("sender",Encoding.UTF8.GetBytes("authService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);    
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_authResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("login")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("authService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            default: 
                                _consumer.Commit(consumeResult);
                                break;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                if (ex is MyKafkaException)
                {
                    _logger.LogError(ex,"Consumer error");
                }
                else
                {
                    _logger.LogError(ex,"Unhandled error");
                }
            }
        }
    }
}