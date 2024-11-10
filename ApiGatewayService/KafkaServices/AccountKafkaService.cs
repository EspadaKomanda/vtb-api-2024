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
                            case "AccountAccessData":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _accountService.AccountAccessData(JsonConvert.DeserializeObject<AccountAccessDataRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("AccountAccessData")),
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
                                     _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("AccountAccessData")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "BeginPasswordReset":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _accountService.BeginPasswordReset(JsonConvert.DeserializeObject<BeginPasswordResetRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("BeginPasswordReset")),
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
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("BeginPasswordReset")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "BeginRegistration":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _accountService.BeginRegistration(JsonConvert.DeserializeObject<BeginRegistrationRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("BeginRegistration")),
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
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("BeginRegistration")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "ChangePassword":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _accountService.ChangePassword(JsonConvert.DeserializeObject<ChangePasswordRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("ChangePassword")),
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
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("ChangePassword")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "CompletePasswordReset":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key, 
                                        Value = JsonConvert.SerializeObject(await _accountService.CompletePasswordReset(JsonConvert.DeserializeObject<CompletePasswordResetRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("CompletePasswordReset")),
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
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("CompletePasswordReset")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "CompleteRegistration":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _accountService.CompleteRegistration(JsonConvert.DeserializeObject<CompleteRegistrationRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("CompleteRegistration")),
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
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("CompleteRegistration")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }   
                                break;
                            case "ResendPasswordResetCode":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value  = JsonConvert.SerializeObject(await _accountService.ResendPasswordResetCode(JsonConvert.DeserializeObject<ResendPasswordResetCodeRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("ResendPasswordResetCode")),
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
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("ResendPasswordResetCode")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "ResendRegistrationCode":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key, 
                                        Value = JsonConvert.SerializeObject( await _accountService.ResendRegistrationCode(JsonConvert.DeserializeObject<ResendRegistrationCodeRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("ResendRegistrationCode")),
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
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("ResendRegistrationCode")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }  
                                break;
                            case "VerifyPasswordResetCode":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _accountService.VerifyPasswordResetCode(JsonConvert.DeserializeObject<VerifyPasswordResetCodeRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("VerifyPasswordResetCode")),
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
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("VerifyPasswordResetCode")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("userService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                } 
                                break;
                            case "VerifyRegistrationCode":
                                try
                                {
                                    if(await base.Produce(_accountResponseTopic,new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject( await _accountService.VerifyRegistrationCode(JsonConvert.DeserializeObject<VerifyRegistrationCodeRequest>(consumeResult.Message.Value))),
                                        Headers = [
                                            new Header("method",Encoding.UTF8.GetBytes("VerifyRegistrationCode")),
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
                                    _ = await base.Produce(_accountResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("VerifyRegistrationCode")), 
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