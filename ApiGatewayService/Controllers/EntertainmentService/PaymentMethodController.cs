using ApiGatewayService.Services.EntertaimentService.PaymentMethods;
using EntertaimentService.Models.PaymentMethod.Requests;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]/ent")]
public class PaymentMethodController(ILogger<PaymentMethodController> logger, IEntertaimentPaymentMethodService tourPaymentMethodsService) : ControllerBase
{
    private readonly ILogger<PaymentMethodController> _logger = logger;
    private readonly IEntertaimentPaymentMethodService _entertainmentPaymentMethodsService = tourPaymentMethodsService;

    [HttpPost]
    [Route("paymentMethods")]
    public async Task<IActionResult> AddPaymentMethod(AddPaymentMethodEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentPaymentMethodsService.AddPaymentMethod(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch]
    [Route("paymentMethods")]
    public async Task<IActionResult> UpdatePaymentMethod(UpdatePaymentMethodEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentPaymentMethodsService.UpdatePaymentMethod(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("paymentMethods/{paymentMethodId}")]
    public async Task<IActionResult> GetPaymentMethod([FromRoute] long paymentMethodId)
    {
        GetPaymentMethodEntertainmentRequest request = new(){PaymentMethodId = paymentMethodId};
        try
        {
            var result = await _entertainmentPaymentMethodsService.GetPaymentMethod(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("paymentMethods")]
    public async Task<IActionResult> RemovePaymentMethod(RemovePaymentMethodEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentPaymentMethodsService.RemovePaymentMethod(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }
    
    // [HttpGet]
    // [Route("paymentMethods/page/{pageId}")]
    // public async Task<IActionResult> GetPaymentMethods([FromRoute] int pageId)
    // {
    //     // FIXME: model and method not found
    //     GetPaymentMethodsRequest request = new(){PageId = pageId};
    //     try
    //     {
    //         var result = await _tourPaymentMethodsService.GetPaymentMethods(request);
    //         return Ok(result);
    //     }
    //     catch(Exception ex)
    //     {
    //         if(ex is MyKafkaException)
    //         {
    //             return StatusCode(500, ex.Message);
    //         }
    //         return BadRequest(ex.Message);
    //     }
    // }
}