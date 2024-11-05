using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiGatewayService.Exceptions.UserService.Account;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using UserService.Models.Account.Requests;
using UserService.Models.Account.Responses;

namespace ApiGatewayService.Services.User
{
    public class UserService(ILogger<UserService> logger, KafkaRequestService kafkaRequestService) : IUserService
    {
        private readonly ILogger<UserService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;

        public async Task<AccountAccessDataResponse> AccountAccessData(AccountAccessDataRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("AccountAccessData")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<AccountAccessDataResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new AccountAccessDataException("Message not recieved");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BeginPasswordResetResponse> BeginPasswordReset(BeginPasswordResetRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("BeginPasswordReset")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<BeginPasswordResetResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new BeginPasswordResetException("Message not recieved");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<BeginRegistrationResponse> BeginRegistration(BeginRegistrationRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("BeginRegistration")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<BeginRegistrationResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new BeginRegistrationException("Message not recieved");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("ChangePassword")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<ChangePasswordResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new ChangePasswordException("Message not recieved");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<CompletePasswordResetResponse> CompletePasswordReset(CompletePasswordResetRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("CompletePasswordReset")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<CompletePasswordResetResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new CompletePasswordResetException("Message not recieved");
            }            
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<CompleteRegistrationResponse> CompleteRegistration(CompleteRegistrationRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("CompleteRegistration")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<CompleteRegistrationResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new CompleteRegistrationException("Message not recieved");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<ResendPasswordResetCodeResponse> ResendPasswordResetCode(ResendPasswordResetCodeRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("ResendPasswordResetCode")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<ResendPasswordResetCodeResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new ResendPasswordResetCodeException("Message not recieved");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<ResendRegistrationCodeResponse> ResendRegistrationCode(ResendRegistrationCodeRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("ResendRegistrationCode")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<ResendRegistrationCodeResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new ResendRegistrationCodeException("Message not recieved");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<VerifyPasswordResetCodeResponse> VerifyPasswordResetCode(VerifyPasswordResetCodeRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("VerifyPasswordResetCode")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<VerifyPasswordResetCodeResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new VerifyPasswordResetCodeException("Message not recieved");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<VerifyRegistrationCodeResponse> VerifyRegistrationCode(VerifyRegistrationCodeRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("VerifyRegistrationCode")),
                        new Header("sender",Encoding.UTF8.GetBytes("ApiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce("userServiceAccountsRequests",message,"userServiceAccountsResponses"))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<VerifyRegistrationCodeResponse>(messageId.ToString(),"userServiceAccountsResponses");
                }
                throw new VerifyRegistrationCodeException("Message not recieved");
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}