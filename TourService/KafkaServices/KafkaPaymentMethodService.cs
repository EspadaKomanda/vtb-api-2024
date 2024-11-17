using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.Kafka.Utils;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;
using TourService.Models.PaymentMethod.Requests;
using TourService.Models.PaymentMethod.Responses;
using TourService.Services.PaymentMethodService;

namespace TourService.KafkaServices
{
    public class KafkaPaymentMethodService : KafkaService
    {
        private readonly string _paymentMethodRequestTopic = Environment.GetEnvironmentVariable("PAYMENTMETHOD_REQUEST_TOPIC") ?? "paymentMethodRequestTopic";
        private readonly string _paymentMethodResponseTopic = Environment.GetEnvironmentVariable("PAYMENTMETHOD_RESPONSE_TOPIC") ?? "paymentMethodResponseTopic"; 
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMapper _mapper;

        public KafkaPaymentMethodService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager, IPaymentMethodService paymentMethodService, IMapper mapper) : base(logger, producer, kafkaTopicManager)
        {
            _paymentMethodService = paymentMethodService;
            _mapper = mapper;
            base.ConfigureConsumer(_paymentMethodRequestTopic);
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
                            case "addPaymentMethod":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<AddPaymentMethodRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_paymentMethodResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(new AddPaymendMethodResponse() 
                                            {
                                                PaymentMethodId = await _paymentMethodService.AddPaymentMethod(new Database.Models.PaymentMethod()
                                                {
                                                    Name = request.PaymentMethodName,
                                                    PaymentVariants = _mapper.Map<List<Database.Models.PaymentVariant>>(request.PaymentVariants)
                                                })
                                            }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("addPaymentMethod")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                    _ = await base.Produce(_paymentMethodResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("addPaymentMethod")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "getPaymentMethod":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetPaymentMethodRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_paymentMethodResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetPaymentMethodResponse(){ 
                                                    PaymentMethod = await _paymentMethodService.GetPaymentMethod(result)
                                                    }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getPaymentMethod")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                    _ = await base.Produce(_paymentMethodResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getPaymentMethod")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "removePaymentMethod":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<Models.PaymentMethod.Requests.RemovePaymentMethodRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_paymentMethodResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new RemovePaymentMethodResponse(){ 
                                                    IsSuccess = _paymentMethodService.RemovePaymentMethod(result)
                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("removePaymentMethod")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                    _ = await base.Produce(_paymentMethodResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("removePaymentMethod")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "updatePaymentMethod":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UpdatePaymentMethodRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_paymentMethodResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new UpdatePaymentMethodResponse(){
                                                     IsSuccess =_paymentMethodService.UpdatePaymentMethod(result)
                                                    }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("updatePaymentMethod")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                    _ = await base.Produce(_paymentMethodResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("updatePaymentMethod")),
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")),
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