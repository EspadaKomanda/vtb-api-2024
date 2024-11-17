using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using EntertaimentService.Kafka.Utils;
using MailService.Models.Mail.Requests;
using MailService.Services.Mailer;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;

namespace MailService.KafkaServices
{
    public class KafkaMailService : KafkaService
    {
        private readonly string _mailRequestTopic = Environment.GetEnvironmentVariable("MAIL_REQUEST_TOPIC") ?? "mailRequestTopic";
        private readonly string _mailResponseTopic = Environment.GetEnvironmentVariable("MAIL_RESPONSE_TOPIC") ?? "mailResponseTopic";
        private readonly IMailerService _mailerService;
        public KafkaMailService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager, IMailerService mailerService) : base(logger, producer, kafkaTopicManager)
        {
            _mailerService = mailerService;
            base.ConfigureConsumer(_mailRequestTopic);
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
                            case "sendMail":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<SendMailRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_mailResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                _mailerService.SendMail(request)
                                            ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("sendMail")),
                                                new Header("sender",Encoding.UTF8.GetBytes("mailService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                    throw new RequestValidationException("Request validation error");
                                }
                                catch (Exception e)
                                {
                                    
                                     _ = await base.Produce(_mailResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("sendMail")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("mailService")), 
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
