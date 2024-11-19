using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;
using UserService.Models;
using UserService.Models.Account.Requests;
using UserService.Services.Account;

namespace UserService.KafkaServices
{
    public class AccountKafkaService : KafkaService
    {
        private readonly string _accountResponseTopic = Environment.GetEnvironmentVariable("ACCOUNT_RESPONSE_TOPIC") ?? "userServiceAccountsResponses";
        private readonly string _accountRequestTopic = Environment.GetEnvironmentVariable("ACCOUNT_REQUEST_TOPIC") ?? "userServiceAccountsRequests";
        private readonly IAccountService _accountService;
        public AccountKafkaService(
            ILogger<KafkaService> logger,
            IAccountService accountService,
            IProducer<string,string> producer,
            KafkaTopicManager kafkaTopicManager) : base(logger, producer, kafkaTopicManager)
        {
            _accountService = accountService;
            base.ConfigureConsumer(_accountRequestTopic);
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
                            case "accountAccessData":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<AccountAccessDataRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(await _accountService.AccountAccessData(result) ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("accountAccessData")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {

                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                   
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("accountAccessData")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "beginPasswordReset":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<BeginPasswordResetRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( await _accountService.BeginPasswordReset(result)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("beginPasswordReset")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }

                                    }
                                    _logger.LogError("Invalid request");
                                   
                                }
                                catch (Exception e)
                                {
                                    
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("beginPasswordReset")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "beginRegistration":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<BeginRegistrationRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( await _accountService.BeginRegistration(result)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("beginRegistration")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                   
                                }
                                catch (Exception e)
                                {
                                    
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("beginRegistration")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "changePassword":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<ChangePasswordRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {

                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( await _accountService.ChangePassword(result)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("changePassword")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                    
                                }
                                catch (Exception e)
                                {
                                    
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("changePassword")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "completePasswordReset":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<CompletePasswordResetRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key, 
                                            Value = JsonConvert.SerializeObject(await _accountService.CompletePasswordReset(result)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("completePasswordReset")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                   
                                }
                                catch (Exception e)
                                {
                                    
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("completePasswordReset")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "completeRegistration":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<CompleteRegistrationRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( await _accountService.CompleteRegistration(result)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("completeRegistration")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                   
                                }
                                catch (Exception e)
                                {
                                   
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("completeRegistration")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }   
                                break;
                            case "resendPasswordResetCode":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<ResendPasswordResetCodeRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value  = JsonConvert.SerializeObject(await _accountService.ResendPasswordResetCode(result)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("resendPasswordResetCode")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("resendPasswordResetCode")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "resendRegistrationCode":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<ResendRegistrationCodeRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {

                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key, 
                                            Value = JsonConvert.SerializeObject( await _accountService.ResendRegistrationCode(result)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("resendRegistrationCode")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                    
                                }   
                                catch (Exception e)
                                {
                                    
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("resendRegistrationCode")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }  
                                break;
                            case "verifyPasswordResetCode":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<VerifyPasswordResetCodeRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( await _accountService.VerifyPasswordResetCode(result)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("verifyPasswordResetCode")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                   
                                }
                                catch (Exception e)
                                {
                                    
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("verifyPasswordResetCode")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                } 
                                break;
                            case "verifyRegistrationCode":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<VerifyRegistrationCodeRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( await _accountService.VerifyRegistrationCode(result)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("verifyRegistrationCode")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                   
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("verifyRegistrationCode")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }    
                                break;
                            case "getUser":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<GetUserRequest>(consumeResult.Message.Value);
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( await _accountService.GetUser(request.UserId,request)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getUser")),
                                                new Header("sender",Encoding.UTF8.GetBytes("userService")),
                                            ]
                                        }))
                                        {
                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                     _logger.LogError("Invalid request");
                                   
                                }
                                catch (Exception e)
                                {
                                    
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getUser")), 
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