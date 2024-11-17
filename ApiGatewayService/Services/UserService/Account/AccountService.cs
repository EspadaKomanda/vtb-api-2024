using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiGatewayService.Exceptions.UserService.Account;
using ApiGatewayService.Models.UserService.Account.Requests;
using ApiGatewayService.Models.UserService.Account.Responses;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using UserService.Models.Account.Requests;
using UserService.Models.Account.Responses;

namespace ApiGatewayService.Services.UserService.Account
{
    public class AccountService(ILogger<AccountService> logger, KafkaRequestService kafkaRequestService) : IAccountService
    {
        private readonly ILogger<AccountService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly string requestTopic = Environment.GetEnvironmentVariable("USER_SERVICE_ACCOUNTS_REQUESTS");
        private readonly string responseTopic = Environment.GetEnvironmentVariable("USER_SERVICE_ACCOUNTS_RESPONSES");
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
        public async Task<AccountAccessDataResponse> AccountAccessData(AccountAccessDataRequest request)
        {
            try
            {
                return await SendRequest<AccountAccessDataRequest,AccountAccessDataResponse>("accountAccessData",request);
            }
            catch(Exception)
            {
                _logger.LogError("Error in AccountAccessData");
                throw;
            }
        }

        public async Task<BeginPasswordResetResponse> BeginPasswordReset(BeginPasswordResetRequest request)
        {
            try
            {
                return await SendRequest<BeginPasswordResetRequest,BeginPasswordResetResponse>("beginPasswordReset",request);
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
                return await SendRequest<BeginRegistrationRequest,BeginRegistrationResponse>("beginRegistration",request);
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
                return await SendRequest<ChangePasswordRequest,ChangePasswordResponse>("changePassword",request);
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
                return await SendRequest<CompletePasswordResetRequest,CompletePasswordResetResponse>("completePasswordReset",request);
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
                return await SendRequest<CompleteRegistrationRequest,CompleteRegistrationResponse>("completeRegistration",request);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<GetUserResponse> GetUser(GetUserRequest request)
        {
            try
            {
                return await SendRequest<GetUserRequest,GetUserResponse>("getUser",request);
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
                return await SendRequest<ResendPasswordResetCodeRequest,ResendPasswordResetCodeResponse>("resendPasswordResetCode",request);
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
                return await SendRequest<ResendRegistrationCodeRequest,ResendRegistrationCodeResponse>("resendRegistrationCode",request);
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
                return await SendRequest<VerifyPasswordResetCodeRequest,VerifyPasswordResetCodeResponse>("verifyPasswordResetCode",request);
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
                return await SendRequest<VerifyRegistrationCodeRequest,VerifyRegistrationCodeResponse>("verifyRegistrationCode",request);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}