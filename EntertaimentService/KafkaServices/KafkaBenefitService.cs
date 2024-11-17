using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using EntertaimentService.Kafka;
using EntertaimentService.Kafka.Utils;
using EntertaimentService.KafkaException;
using EntertaimentService.KafkaException.ConsumerException;
using EntertaimentService.Models.Benefits;
using EntertaimentService.Models.Benefits.Responses;
using EntertaimentService.Services.BenefitService;
using TourService.Models.Benefits.Responses;
using TourService.Kafka;

namespace EntertaimentService.KafkaServices
{
    public class KafkaBenefitService : KafkaService
    {
        //FIXME: If mykafkaException i have to reconfigure the producer
        private readonly string _benefitResponseTopic = Environment.GetEnvironmentVariable("BENEFITRESPONSE_TOPIC") ?? "benefit-response-topic";
        private readonly string _benefitRequestTopic = Environment.GetEnvironmentVariable("BENEFITREQUEST_TOPIC") ?? "benefit-request-topic";
        private readonly IBenefitService _benefitService;
        public KafkaBenefitService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager, IBenefitService benefitService) : base(logger, producer, kafkaTopicManager)
        {
            _benefitService = benefitService;
            base.ConfigureConsumer(_benefitRequestTopic);
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
                            case "addBenefit":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<AddBenefitRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_benefitResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(new AddBenefitResponse() 
                                            {
                                                BenefitId = await _benefitService.AddBenefit(new Database.Models.Benefit()
                                                {
                                                    Name = request.BenefitName
                                                })
                                            }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("addBenefit")),
                                                new Header("sender",Encoding.UTF8.GetBytes("entertaimentService"))
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
                                    _ = await base.Produce(_benefitResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("addBenefit")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("entertaimentService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "getBenefit":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetBenefitRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_benefitResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetBenefitResponse()
                                                {
                                                    Benefit =await _benefitService.GetBenefit(result)
                                                }),
                                            
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getBenefit")),
                                                new Header("sender",Encoding.UTF8.GetBytes("entertaimentService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                    throw new RequestValidationException("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    
                                     _ = await base.Produce(_benefitResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getBenefit")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("entertaimentService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "getBenefits":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetBenefitsRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_benefitResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetBenefitsResponse()
                                                {
                                                    Benefits = _benefitService.GetBenefits(result).ToList()
                                                }
                                            ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getBenefits")),
                                                new Header("sender",Encoding.UTF8.GetBytes("entertaimentService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                    throw new RequestValidationException("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    
                                     _ = await base.Produce(_benefitResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getBenefits")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("entertaimentService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "removeBenefit":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<RemoveBenefitRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_benefitResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new RemoveBenefitResponse(){
                                                    IsSuccess = _benefitService.RemoveBenefit(result)
                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("removeBenefit")),
                                                new Header("sender",Encoding.UTF8.GetBytes("entertaimentService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }

                                    }
                                    _logger.LogError("Request validation error");
                                    throw new RequestValidationException("Invalid request");
                                }
                                catch (Exception e)
                                {
                                   
                                     _ = await base.Produce(_benefitResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("removeBenefit")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("entertaimentService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "updateBenefit":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UpdateBenefitRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_benefitResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new UpdateBenefitResponse(){
                                                    IsSuccess = _benefitService.UpdateBenefit(result)
                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("updateBenefit")),
                                                new Header("sender",Encoding.UTF8.GetBytes("entertaimentService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                    throw new RequestValidationException("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    
                                     _ = await base.Produce(_benefitResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("updateBenefit")),
                                            new Header("sender", Encoding.UTF8.GetBytes("entertaimentService")),
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
                }
                else
                {
                    _logger.LogError(ex,"Unhandled error");
                    
                }
            }
        }
    }
}